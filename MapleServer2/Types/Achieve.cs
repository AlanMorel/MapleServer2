using System;
using System.Collections.Generic;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types
{
    public class Achieve
    {
        public int Id { get; private set; }
        public int CurrentGrade { get; private set; }
        public int MaxGrade { get; private set; }
        public long Counter { get; private set; }
        public long Condition { get; private set; }
        public List<long> Timestamps { get; private set; }

        public Achieve(int achieveId, int grade = 0, int counter = 0, List<long> timestamps = null)
        {
            Id = achieveId;
            CurrentGrade = grade;
            Counter = counter;
            Timestamps = timestamps ?? new List<long>();
            MaxGrade = AchieveMetadataStorage.GetNumGrades(Id);
            Condition = AchieveMetadataStorage.GetCondition(Id, CurrentGrade);
        }

        public AchievePacket.GradeStatus GetGradeStatus()
        {
            return Condition == 0 ? AchievePacket.GradeStatus.FinalGrade : AchievePacket.GradeStatus.NotFinalGrade;
        }

        public void AddCounter(long amount)
        {
            Counter += amount;
            // level up achievement if counter reached condition of next grade
            if ((Condition != 0) && (Counter >= Condition))
            {
                // level up but not fully completed
                if (CurrentGrade < MaxGrade)
                {
                    CurrentGrade++;
                    Condition = AchieveMetadataStorage.GetCondition(Id, CurrentGrade);
                }
                // level up and fully completed
                else
                {
                    Condition = 0;
                }

                Timestamps.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }
        }
    }
}
