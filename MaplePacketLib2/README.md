MaplePacketLib2
===============
Packet Library for MapleStory2

Credits to [@EricSoftTM](https://github.com/EricSoftTM) for packet encryption algos

### General Usage:

- SIV: Send IV
- RIV: Recv IV
- BLOCK_IV: Determines cipher ordering

Sending Packets

```C#
MapleCipher sendCipher = MapleCipher.Encryptor(VERSION, SIV, BLOCK_IV);
...
// Handshake is NOT encrypted
PacketWriter writer = PacketWriter.Of(HANDSHAKE_OPCODE);
Packet handshake = sendCipher.WriteHeader(handshake.ToArray());
SendPacket(handshake);
...
PacketWriter writer = PacketWriter.Of(OPCODE);
writer.Write(DATA);

Packet encryptedSendPacket = sendCipher.Transform(writer.ToArray());
SendPacket(encryptedSendPacket);
```

Receiving Packets

```C#
MapleCipher recvCipher = MapleCipher.Decryptor(VERSION, RIV, BLOCK_IV);
MapleStream stream = new MapleStream();
...
while (IS_READING) {
    var data = bytes from network;
    stream.Write(data);
    
    byte[] packetBuffer = mapleStream.Read();
    if (packetBuffer != null) {
        Packet decryptedPacket = recvCipher.Transform(packetBuffer);
        OnPacket(decryptedPacket);
    }
}
```
