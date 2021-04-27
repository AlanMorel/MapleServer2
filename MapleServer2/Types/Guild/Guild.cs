﻿using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Guild
    {
        // TODO: Add ranks, rank names, Member contribution
        public long Id { get; }
        public string Name { get; set; }
        public long CreationTimestamp { get; set; }
        public Player Leader { get; set; }
        public byte Capacity { get; set; }
        public List<GuildMember> Members = new List<GuildMember>();
        public List<GuildRank> Ranks;
        public List<GuildBuff> Buffs = new List<GuildBuff>();
        public List<GuildService> Services = new List<GuildService>();
        public List<Item> GiftBank = new List<Item>();
        public int Funds { get; set; }
        public int Exp { get; set; }
        public bool Searchable { get; set; }
        public string Notice = "";
        public string Emblem = "";
        public int FocusAttributes;
        public int HouseRank;
        public int HouseTheme;
        public List<GuildApplication> Applications = new List<GuildApplication>();

        public Guild(string name, Player player)
        {
            GuildMember leader = new GuildMember(player);
            GuildPropertyMetadata property = GuildPropertyMetadataStorage.GetMetadata(0);

            Id = GuidGenerator.Long();
            Name = name;
            Capacity = (byte) property.Capacity;
            Leader = player;
            Members.Add(leader);
            Exp = 0;
            Funds = 0;
            Searchable = true;
            HouseRank = 1;
            HouseTheme = 1;
            Ranks = new List<GuildRank>() {
                new GuildRank("Master", 4095),
                new GuildRank("Jr. Master"),
                new GuildRank("Member 1"),
                new GuildRank("Member 2"),
                new GuildRank("New Member 1"),
                new GuildRank("New Member 2")
            };
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            AddGuildBuffs();

            player.GuildId = Id;
            player.GuildMemberId = leader.Id;
            leader.Rank = 0;
        }

        public void AddMember(Player player)
        {
            GuildMember member = new GuildMember(player);
            player.GuildId = Id;
            player.GuildMemberId = member.Id;
            Members.Add(member);
        }

        public void RemoveMember(Player player)
        {
            GuildMember member = Members.First(x => x.Player == player);

            Members.Remove(member);
            player.GuildId = 0;
            player.GuildMemberId = 0;
        }

        public void AssignNewLeader(Player oldLeader, Player newLeader)
        {
            GuildMember newLeadMember = Members.First(x => x.Player == newLeader);
            GuildMember oldLeadMember = Members.First(x => x.Player == oldLeader);

            Members.Insert(0, newLeadMember);
            Members.Remove(oldLeadMember);
            Members.Add(oldLeadMember);
        }

        public void ModifyFunds(GameSession session, GuildPropertyMetadata property, int funds)
        {
            if (funds > 0)
            {
                if (Funds >= property.FundMax)
                {
                    return;
                }
                Funds += funds;

                if (Funds >= property.FundMax)
                {
                    Funds = property.FundMax;
                }
            }

            else
            {
                Funds += funds;
            }

            BroadcastPacketGuild(GuildPacket.UpdateGuildFunds(Funds));
            session.Send(GuildPacket.UpdateGuildStatsNotice(0, funds));
        }

        public void AddExp(GameSession session, int expGain)
        {
            Exp += expGain;
            BroadcastPacketGuild(GuildPacket.UpdateGuildExp(Exp));
            session.Send(GuildPacket.UpdateGuildStatsNotice(expGain, 0));
        }

        public void LevelService(int serviceId, int currentLevel)
        {
            GuildService service = Services.FirstOrDefault(x => x.Id == serviceId);
            if (service == null)
            {
                GuildService newService = new GuildService(serviceId, currentLevel + 1);
                Services.Add(newService);
                return;
            }
            service.Level++;
        }

        private void AddGuildBuffs()
        {
            List<int> buffIds = GuildBuffMetadataStorage.GetBuffList();
            foreach (int buffId in buffIds)
            {
                Buffs.Add(new GuildBuff(buffId));
            }
        }

        public void BroadcastPacketGuild(Packet packet, GameSession sender = null)
        {
            BroadcastGuild(session =>
            {
                if (session == sender)
                {
                    return;
                }

                session.Send(packet);
            });
        }

        public void BroadcastGuild(Action<GameSession> action)
        {
            IEnumerable<GameSession> sessions = GetSessions();
            lock (sessions)
            {
                foreach (GameSession session in sessions)
                {
                    action?.Invoke(session);
                }
            }
        }

        private List<GameSession> GetSessions()
        {
            return Members.Where(x => x.Player.Session.Connected()).Select(x => x.Player.Session).ToList();
        }
    }

    [Flags]
    public enum GuildFocus
    {
        Social = 1,
        HuntingParties = 2,
        TrophyCollection = 4,
        Dungeons = 8,
        HomeDesign = 10,
        PvP = 20,
        WorkshopTemplates = 40,
        GuildArcade = 80,
        Weekdays = 256,
        Mornings = 512,
        Weekends = 1024,
        Evenings = 2048,
        Teens = 4096,
        Thirties = 8192,
        Twenties = 16384,
        Other = 32768
    }
}
