using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using System.Collections.Generic;

namespace MapleServer2.Packets
{
    public static class PartyPacket
    {
        public static Packet SendInvite(Player recipient, Player sender)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(11) //Mode?
                .WriteUnicodeString(sender.Name)
                .WriteShort(3843)
                .WriteByte()
                .WriteByte();

            return pWriter;
        }

        //Creates a backend party
        public static Packet CreateParty(Player p)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0009) //Create new party I think. Sent to player inviting someone to party
                .WriteByte(2) //unkown
                .WriteInt() //unknown
                .WriteLong(p.CharacterId)
                .WriteByte(1) //Party member count?
                .WriteByte();

            CharacterListPacket.WriteCharacter(p, pWriter);
            pWriter.WriteLong();
            JobPacket.WriteSkills(pWriter, p);

            return pWriter;
        }

        //UI PLAYER JOIN
        public static Packet JoinParty(Player p)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0002); //Add player to party UI

            CharacterListPacket.WriteCharacter(p, pWriter);
            pWriter.WriteLong();
            JobPacket.WriteSkills(pWriter, p);

            return pWriter;
        }

        public static Packet JoinParty2(Player p)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x000D) //Pretty sure this is the party update function, call after UI joinparty to update their location, afk status, etc.
                .WriteLong(p.CharacterId);

            CharacterListPacket.WriteCharacter(p, pWriter);
            pWriter.WriteLong();
            JobPacket.WriteSkills(pWriter, p);

            return pWriter;
        }

        public static Packet JoinParty3(Player party1, Player party2)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0009) //Creates party on the backend with the 2 members
                .WriteByte(2) //unkown
                .WriteInt()//unknown
                .WriteLong(party1.CharacterId)
                .WriteByte(2) //Party member count?
                .WriteByte();

            CharacterListPacket.WriteCharacter(party1, pWriter);
            pWriter.WriteLong();
            WriteSkills(pWriter, party1);
            CharacterListPacket.WriteCharacter(party2, pWriter);
            pWriter.WriteLong();
            JobPacket.WriteSkills(pWriter, party2);

            return pWriter;
        }


        public static Packet UpdateHitpoints(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0013)
                .WriteLong(player.CharacterId)
                .WriteLong(player.AccountId)
                .WriteInt(player.Stats.Hp.Total)
                .WriteInt(player.Stats.CurrentHp.Min)
                .WriteShort();
            return pWriter;
        }

        public static Packet SetPartyLeader(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0008)
                .WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet LeaveParty(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0004)
                .WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet WriteSkills(PacketWriter pWriter, Player character)
        {
            // Get skills
            Dictionary<int, Skill> skills = character.SkillTabs[0].Skills; // Get first skill tab skills only for now, uncertain of how to have multiple skill tabs

            // Ordered list of skill ids (must be sent in this order)
            int[] ids = character.SkillTabs[0].Order;
            int countId = ids[ids.Length - 8]; // 8th to last skill id

            pWriter.WriteByte((byte)(ids.Length - 8)); // Skill count minus 8

            // List of skills for given tab in format (byte zero) (byte learned) (int skill_id) (int skill_level) (byte zero)
            foreach (int id in ids)
            {
                if (id == countId)
                {
                    pWriter.WriteByte(8); // Write that there are 8 skills left
                }
                pWriter.WriteByte();
                pWriter.WriteByte(skills[id].Learned);
                pWriter.WriteInt(id);
                pWriter.WriteInt((int)skills[id].Level);
                pWriter.WriteByte();
            }

            pWriter.WriteByte(); // Ends with zero short

            return pWriter;
        }

    }
}