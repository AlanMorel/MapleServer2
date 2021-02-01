using System;
using System.Collections.Generic;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Achieve
    {
        public Player Owner { get; private set; }
        public int Id { get; private set; }
        public int CurrentGrade { get; private set; }   // grade trying to achieve (value from 1 to MaxGrade)
        public int MaxGrade { get; private set; }
        public long Counter { get; private set; }    // counter to reach next grade
        public long Condition { get; private set; }    // counter value needed to reach next grade (-1 if fully completed)
        public List<long> Timestamps { get; private set; }  // stored in ascending order of grades as seconds since epoch time

        public Achieve(int achieveId, int grade = 0, int counter = 0, List<long> timestamps = null)
        {
            Id = achieveId;
            CurrentGrade = grade;
            Counter = counter;
            Timestamps = timestamps ?? new List<long>();
            MaxGrade = AchieveMetadataStorage.GetNumGrades(Id);
            Condition =  AchieveMetadataStorage.GetCondition(Id, CurrentGrade);
        }

        public void AddCounter(int amount)
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
                    Condition = 0;

                Timestamps.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }
        }
    }
}
