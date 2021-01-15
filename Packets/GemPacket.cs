using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    public static class GemPacket
    {
        private enum GemMode : byte
        {
            EquipItem = 0x00,
            UnequipItem = 0x01
        }

        public static Packet EquipItem(GameSession session, Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GEM);

            pWriter.WriteMode(GemMode.EquipItem);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteInt(item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteInt(item.Rarity);
            pWriter.WriteByte((byte) (session.Player.Badges.Count - 1));

            // TODO: Figure out how to fit this into WriteItem
            // It's strange though because it only writes the special values for opcode 0x0081, not for inventory add/remove opcodes
            // Start WriteItem
            pWriter.WriteInt(); // Amount
            pWriter.WriteInt(); // Unknown
            pWriter.WriteInt(-1); // Unknown (-1)
            pWriter.WriteLong(item.CreationTime); // Creation Time
            pWriter.WriteLong(); // Expiration Time
            pWriter.WriteLong(); // Unknown
            pWriter.WriteInt(); // Times Attributes Changed
            pWriter.WriteInt(); // Unknown
            pWriter.WriteBool(false); // Bool isLocked
            pWriter.WriteLong(); // Unlock Time
            pWriter.WriteShort(); // Remaining Glamour Forges
            pWriter.WriteByte(); // Unknown
            pWriter.WriteInt(); // Unknown

            // Item Appearance
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt(-1);
            pWriter.WriteInt();
            pWriter.WriteByte();

            // Item Stats
            pWriter.WriteShort(); // basic attributes count?
            pWriter.WriteShort(); // special attribute?
            pWriter.WriteInt(); // special attribute?
            pWriter.WriteShort(); // basic attribute block?
            pWriter.WriteShort(); // basic attribute block?
            pWriter.WriteInt(); // basic attribute block?
            pWriter.WriteShort(); // bonus attributes count?
            pWriter.WriteShort(); // special attribute?
            pWriter.WriteInt(); // special attribute?
            // 6 more basic attribute blocks?
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();

            // Continue WriteItem
            pWriter.WriteInt(); // Enchants
            pWriter.WriteInt(); // EnchantExp
            pWriter.WriteByte(); // Enchant based peachy charges, if false always require 10 charges
            pWriter.WriteLong(); // Unknown
            pWriter.WriteInt(); // Unknown
            pWriter.WriteInt(); // Unknown
            pWriter.WriteBool(item.CanRepackage);
            pWriter.WriteInt(); // Charges

            // Item Stat Diff
            pWriter.WriteByte(); // General stat diff count
            pWriter.WriteInt(); // Unknown
            pWriter.WriteInt(); // Stat diff count 
            pWriter.WriteInt(); // Bonus stat diff count 

            // Now deviate from WriteItem
            pWriter.WriteBool(true); // Bool isBadge?
            pWriter.WriteByte((byte) item.GemSlot);
            pWriter.WriteUnicodeString(item.Id.ToString()); // Item id as string (not included in WriteItem)

            // Continue WriteItem
            // Skip template
            // Skip pet
            pWriter.WriteInt(); // Transfer Flag (08 00 00 00 => binds)
            pWriter.WriteByte(); // Unknown (1)
            pWriter.WriteInt(); // Unknown
            pWriter.WriteInt(); // Unknown
            pWriter.WriteByte(); // Unknown
            pWriter.WriteByte(0); // Unknown (1)

            bool isCharBound = item.Owner != null;
            pWriter.WriteBool(isCharBound);
            if (isCharBound)
            {
                pWriter.WriteLong(session.Player.CharacterId); // Bound character id
                pWriter.WriteUnicodeString(session.Player.Name); // Bound character name
            }

            pWriter.WriteByte(); // Unknown

            // Item Sockets
            pWriter.WriteByte(); // Total sockets

            // Continue WriteItem
            pWriter.WriteLong(item.PairedCharacterId); // Paired with character id
            if (item.PairedCharacterId != 0)
            {
                pWriter.WriteUnicodeString(""); // Paired with character name
                pWriter.WriteBool(false); // Unknown
            }

            pWriter.WriteLong(); // Unknown
            pWriter.WriteUnicodeString(""); // Unknown

            return pWriter;
        }

        public static Packet UnequipItem(GameSession session, byte index)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GEM);

            pWriter.WriteMode(GemMode.UnequipItem);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteByte(index);

            return pWriter;
        }
    }
}
