using System;
using System.Collections.Generic;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Maple2Storage.Enums;
using MapleServer2.Enums;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types
{
    public class Trophy
    {
        public int Id { get; private set; }
        public int NextGrade { get; private set; }
        public int MaxGrade { get; private set; }
        public long Counter { get; private set; }
        public long Condition { get; private set; }
        public bool IsDone { get; private set; }
        public List<long> Timestamps { get; private set; }

        public Trophy(int trophyId, int grade = 1, int counter = 0, List<long> timestamps = null, bool isDone = false)
        {
            Id = trophyId;
            NextGrade = grade;
            Counter = counter;
            Timestamps = timestamps ?? new List<long>();
            MaxGrade = TrophyMetadataStorage.GetNumGrades(Id);
            Condition = TrophyMetadataStorage.GetGrade(Id, NextGrade).Condition;
            IsDone = isDone;
        }

        public TrophyPacket.GradeStatus GetGradeStatus()
        {
            return IsDone ? TrophyPacket.GradeStatus.FinalGrade : TrophyPacket.GradeStatus.NotFinalGrade;
        }

        public void AddCounter(GameSession session, long amount)
        {
            if (IsDone)
            {
                return;
            }
            Counter += amount;

            if (Counter >= Condition)
            {
                ProvideReward(session);
                NextGrade++;
                // level up but not completed
                if (NextGrade <= MaxGrade)
                {
                    Condition = TrophyMetadataStorage.GetGrade(Id, NextGrade).Condition;
                }
                // level up and completed
                else
                {
                    IsDone = true;
                    NextGrade--;
                    string[] categories = TrophyMetadataStorage.GetMetadata(Id).Categories;
                    foreach (string category in categories)
                    {
                        switch (category)
                        {
                            case string s when s.Contains("combat"):
                                session.Player.TrophyCount[0] += 1;
                                break;
                            case string s when s.Contains("adventure"):
                                session.Player.TrophyCount[1] += 1;
                                break;
                            case string s when s.Contains("lifestyle"):
                                session.Player.TrophyCount[2] += 1;
                                break;
                        }
                    }
                }
                Timestamps.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }
        }

        private void ProvideReward(GameSession session)
        {
            TrophyGradeMetadata grade = TrophyMetadataStorage.GetGrade(Id, NextGrade);
            RewardType type = (RewardType) grade.RewardType;
            switch (type)
            {
                case RewardType.Unknown:
                    break;
                case RewardType.itemcoloring:
                    break;
                case RewardType.shop_ride:
                    break;
                case RewardType.title:
                    break;
                case RewardType.beauty_hair:
                    break;
                case RewardType.statPoint:
                    session.Player.StatPointDistribution.AddTotalStatPoints(grade.RewardValue, OtherStatsIndex.Trophy);
                    session.Send(StatPointPacket.WriteTotalStatPoints(session.Player));
                    break;
                case RewardType.skillPoint:
                    break;
                case RewardType.beauty_makeup:
                    break;
                case RewardType.shop_build:
                    break;
                case RewardType.item:
                    break;
                case RewardType.shop_weapon:
                    break;
                case RewardType.dynamicaction:
                    break;
                case RewardType.etc:
                    break;
                case RewardType.beauty_skin:
                    break;
                default:
                    break;
            }
        }
    }
}
