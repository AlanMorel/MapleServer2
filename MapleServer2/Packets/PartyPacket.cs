using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using System.Collections.Generic;
using System.Linq;

namespace MapleServer2.Packets
{
    public static class PartyPacket
    {

        /// <summary>
        /// Sends a party invite to the player. Unsure what the 3843 short is.
        /// </summary>
        /// <param name="sender">The player inviting</param>
        /// <returns></returns>
        public static Packet SendInvite(Player sender)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x000B) //Invite
                .WriteUnicodeString(sender.Name)
                .WriteShort(3843)
                .WriteByte()
                .WriteByte();

            return pWriter;
        }


        /// <summary>
        /// Creates a party with the specified player as the leader. Used when inviting someone to a party when you are not in one.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Packet Create(Player p)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);

            CreatePartyHeader(p, pWriter, 1);

            CharacterListPacket.WriteCharacter(p, pWriter);
            pWriter.WriteLong();
            JobPacket.WriteSkills(pWriter, p);

            return pWriter;
        }

        public static Packet CreateExisting(Player leader, HashSet<Player> members)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);

            CreatePartyHeader(leader, pWriter, (byte)members.Count);

            foreach (Player member in members)
            {
                CharacterListPacket.WriteCharacter(member, pWriter);
                pWriter.WriteLong();
                WriteSkills(pWriter, member);
                if (member != members.Last())
                {
                    pWriter.WriteByte();
                }

            }

            return pWriter;
        }

        //Generates the header code for Create
        public static Packet CreatePartyHeader(Player p, PacketWriter pWriter, byte memberCount)
        {
            pWriter.WriteByte(0x0009) //Creates party on the backend with the 2 members
                .WriteByte(2)
                .WriteInt()
                .WriteLong(p.CharacterId)
                .WriteByte(memberCount) //Party member count?
                .WriteByte();
            return pWriter;
        }



        /// <summary>
        /// Adds the specified player to the party UI, that player needs to be added either with UpdatePlayer or CreateExisting
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Packet Join(Player p)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0002); //Add player to party UI

            CharacterListPacket.WriteCharacter(p, pWriter);
            pWriter.WriteLong();
            JobPacket.WriteSkills(pWriter, p);

            return pWriter;
        }

        /// <summary>
        /// Update existing player or add a new player to the backend party. Does not add them to the UI.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Packet UpdatePlayer(Player p)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x000D) //Pretty sure this is the party update function, call after Join to update their location, afk status, etc.
                .WriteLong(p.CharacterId);

            CharacterListPacket.WriteCharacter(p, pWriter);
            pWriter.WriteLong();
            JobPacket.WriteSkills(pWriter, p);
            return pWriter;
        }



        /// <summary>
        /// Updates the hitpoints on the party UI of the specified player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>

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


        /// <summary>
        /// Sets the given player as party leader.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Packet SetLeader(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0008)
                .WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet Disband()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0007);
            return pWriter;
        }


        /// <summary>
        /// Leaves the party.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Packet Leave(Player player, byte self)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0003)
                .WriteLong(player.CharacterId)
                .WriteByte(self); //0 = Other leaving, 1 = Self leaving
            return pWriter;
        }


        /// <summary>
        /// Kicks player from the party.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Packet Kick(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0004)
                .WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet ReadyCheck(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x002E)
                .WriteLong(player.CharacterId);
            return pWriter;
        }

        //Had to copy this method because of the last short being written.
        private static Packet WriteSkills(PacketWriter pWriter, Player character)
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

            return pWriter;
        }

    }
}