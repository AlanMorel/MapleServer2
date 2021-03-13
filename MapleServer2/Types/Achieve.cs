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
        public int CurrentGrade { get; private set; }   // next grade being achieved; cannot exceed max
        public int MaxGrade { get; private set; }
        public long Counter { get; private set; }
        public long Condition { get; private set; }
        public List<long> Timestamps { get; private set; }

        public Achieve(int achieveId, int grade = 1, int counter = 0, List<long> timestamps = null)
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
            if (Condition == 0)
            {
                return;
            }

            Counter += amount;
            // level up achievement if counter reached condition of next grade
            if (Counter >= Condition)
            {
                CurrentGrade++;
                // level up but not fully completed
                if (CurrentGrade <= MaxGrade)
                {
                    Condition = AchieveMetadataStorage.GetCondition(Id, CurrentGrade);
                }
                // level up and fully completed
                else
                {
                    Condition = 0;
                    CurrentGrade--;
                }

                Timestamps.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }
        }
    }
}
