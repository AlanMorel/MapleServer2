using System.Collections.Immutable;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.PacketHandlers;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Network
{
    public class PacketRouter<T> where T : Session
    {
        private readonly ImmutableDictionary<RecvOp, IPacketHandler<T>> Handlers;
        private readonly ILogger Logger;

        public PacketRouter(IEnumerable<IPacketHandler<T>> packetHandlers, ILogger<PacketRouter<T>> logger)
        {
            Logger = logger;

            ImmutableDictionary<RecvOp, IPacketHandler<T>>.Builder builder = ImmutableDictionary.CreateBuilder<RecvOp, IPacketHandler<T>>();
            foreach (IPacketHandler<T> packetHandler in packetHandlers)
            {
                Register(builder, packetHandler);
            }

            Handlers = builder.ToImmutable();
        }

        public void OnPacket(object sender, Packet packet)
        {
            PacketReader reader = packet.Reader();
            ushort op = reader.ReadUShort();
            IPacketHandler<T> handler = Handlers.GetValueOrDefault((RecvOp) op);
            handler?.Handle(sender as T, reader);
        }

        private static void Register(ImmutableDictionary<RecvOp, IPacketHandler<T>>.Builder builder, IPacketHandler<T> packetHandler)
        {
            builder.Add(packetHandler.OpCode, packetHandler);
        }
    }
}
