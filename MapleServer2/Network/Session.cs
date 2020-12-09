﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using MaplePacketLib2.Crypto;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Extensions;
using Microsoft.Extensions.Logging;
using Pastel;
using System.Reflection;
using System.Linq;

namespace MapleServer2.Network
{
    public abstract class Session : IDisposable
    {
        public const uint VERSION = 12;

        private const int BUFFER_SIZE = 1024;
        private const uint BLOCK_IV = 12; // TODO: should this be variable

        private const int STOP_TIMEOUT = 2000;

        protected abstract SessionType Type { get; }

        public EventHandler<string> OnError;
        public EventHandler<Packet> OnPacket;

        private uint siv;
        private uint riv;

        private Task sendThread;
        private Task recvThread;
        private TcpClient client;
        private NetworkStream networkStream;
        private MapleStream mapleStream;
        private MapleCipher sendCipher;
        private MapleCipher recvCipher;

        // These are unencrypted packets, scheduled to be sent on a single thread.
        private readonly Queue<byte[]> sendQueue;
        private readonly byte[] recvBuffer;
        private readonly CancellationTokenSource source;

        protected readonly ILogger logger;

        private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        protected Session(ILogger<Session> logger)
        {
            this.logger = logger;
            this.sendQueue = new Queue<byte[]>();
            this.recvBuffer = new byte[BUFFER_SIZE];
            this.source = new CancellationTokenSource();
        }

        public void Init([NotNull] TcpClient client)
        {
            // Allow client to close immediately
            client.LingerState = new LingerOption(true, 0);

            byte[] sivBytes = new byte[4];
            byte[] rivBytes = new byte[4];
            rng.GetBytes(sivBytes);
            rng.GetBytes(rivBytes);
            this.siv = BitConverter.ToUInt32(sivBytes);
            this.riv = BitConverter.ToUInt32(rivBytes);

            this.client = client;
            this.networkStream = client.GetStream();
            this.mapleStream = new MapleStream();
            this.sendCipher = MapleCipher.Encryptor(VERSION, siv, BLOCK_IV);
            this.recvCipher = MapleCipher.Decryptor(VERSION, riv, BLOCK_IV);
        }

        public void Dispose()
        {
            Disconnect();
            client?.Dispose();
        }

        public void Disconnect()
        {
            StopThreads();
            if (Connected())
            {
                // Must close socket before network stream to prevent lingering
                client.Client.Close();
                client.Close();
                logger.Debug($"Disconnected client.");
            }
        }

        private void StopThreads()
        {
            source.Cancel();
            sendThread.Wait(STOP_TIMEOUT);
            recvThread.Wait(STOP_TIMEOUT);
        }

        public bool Connected()
        {
            if (client?.Client == null)
            {
                return false;
            }

            Socket socket = client.Client;
            return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);
        }

        public void Start()
        {
            if (sendThread != null || recvThread != null)
            {
                throw new ArgumentException("Session has already been started.");
            }

            recvThread = new Task(() =>
            {
                try
                {
                    PerformHandshake();
                    StartRead();
                }
                catch (SystemException ex)
                {
                    logger.Trace($"Fatal error for session:{this}", ex);
                    Disconnect();
                }
            });
            sendThread = new Task(() =>
            {
                try
                {
                    StartWrite();
                }
                catch (SystemException ex)
                {
                    logger.Trace($"Fatal error for session:{this}", ex);
                    Disconnect();
                }
            });
            sendThread.Start();
            recvThread.Start();
        }

        private void PerformHandshake()
        {
            if (client == null)
            {
                throw new InvalidOperationException("Cannot start a session without a client.");
            }

            PacketWriter pWriter = PacketWriter.Of(SendOp.REQUEST_VERSION);
            pWriter.WriteUInt(VERSION);
            pWriter.WriteUInt(riv);
            pWriter.WriteUInt(siv);
            pWriter.WriteUInt(BLOCK_IV);
            pWriter.WriteByte((byte)Type);

            // No encryption for handshake
            Packet packet = sendCipher.WriteHeader(pWriter.ToArray());
            logger.Debug($"Handshake: {packet}");
            SendRaw(packet);
        }

        public void Send(params byte[] packet)
        {
            lock (sendQueue)
            {
                sendQueue.Enqueue(packet);
            }
        }

        public void Send(Packet packet)
        {
            lock (sendQueue)
            {
                sendQueue.Enqueue(packet.ToArray());
            }
        }

        // Ensures no more communication before sending a final packet.
        public void SendFinal(Packet packet)
        {
            StopThreads();
            SendInternal(packet.ToArray());
        }

        private async void StartRead()
        {
            CancellationToken readToken = source.Token;
            while (!readToken.IsCancellationRequested)
            {
                try
                {
                    int length = await networkStream.ReadAsync(recvBuffer, 0, recvBuffer.Length, readToken);
                    if (length <= 0)
                    {
                        if (!Connected()) return;
                        continue;
                    }

                    mapleStream.Write(recvBuffer, 0, length);
                }
                catch (IOException ex)
                {
                    logger.Error("Exception reading from socket: ", ex);
                    return;
                }

                while (mapleStream.TryRead(out byte[] packetBuffer))
                {
                    Packet packet = recvCipher.Transform(packetBuffer);
                    short opcode = packet.Reader().ReadShort();

                    //Show name of Constant RecvOp
                    var props = typeof(RecvOp).GetFields(BindingFlags.Public | BindingFlags.Static);
                    var wantedProps = props.FirstOrDefault(prop => (ushort)prop.GetValue(opcode) == opcode);

                    if (opcode != 0x12)
                    { // Filtering sync from logs
                        if (wantedProps != null)
                        {
                            logger.Debug($"RECV ({wantedProps.Name}: {packet.Length})".Pastel("#812F2F") + $": {packet}");
                        }
                        else
                        {
                            logger.Debug($"RECV ({String.Format("0x00{0:X}", opcode)}: {packet.Length})".Pastel("#ED9C1A") + $": {packet}");
                        }
                    }
                    OnPacket?.Invoke(this, packet); // handle packet
                }
            }
        }

        private void StartWrite()
        {
            CancellationToken writeToken = source.Token;
            while (!writeToken.IsCancellationRequested)
            {
                lock (sendQueue)
                {
                    while (sendQueue.TryDequeue(out byte[] packet))
                    {
                        SendInternal(packet);
                    }
                }
            }
        }

        private void SendInternal(byte[] packet)
        {
            short opcode = BitConverter.ToInt16(packet, 0);

            //Show name of Constant SendOp
            var props = typeof(SendOp).GetFields(BindingFlags.Public | BindingFlags.Static);
            var wantedProps = props.FirstOrDefault(prop => (ushort)prop.GetValue(opcode) == opcode);

            if (opcode != 0x1C)
            { // Filtering sync from logs{String.Format("0x00{0:X}", opcode)}
                if (wantedProps != null)
                {
                    logger.Debug($"SEND ({wantedProps.Name}: {packet.Length})".Pastel("#197319") + $": {packet.ToHexString(' ')}");
                }
                else
                {
                    logger.Debug($"SEND ({String.Format("0x00{0:X}", opcode)}: {packet.Length})".Pastel("#197319") + $": {packet.ToHexString(' ')}");
                }
            }
            Packet encryptedPacket = sendCipher.Transform(packet);
            SendRaw(encryptedPacket);
        }

        private void SendRaw(Packet packet)
        {
            try
            {
                networkStream.Write(packet.Buffer, 0, packet.Length);
            }
            catch (IOException ex)
            {
                logger.Error("Exception writing to socket: ", ex);
                Disconnect();
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name} from {client?.Client.RemoteEndPoint}";
        }
    }
}