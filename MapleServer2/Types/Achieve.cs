using System;
using System.Collections.Generic;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Achieve
    {
        public Player Owner { get; private set; }
        public int Id { get; private set; }
        public int CurrentGrade {get; private set; }   // grade trying to achieve (value from 1 to MaxGrade)
        public int MaxGrade {get; private set; }
        public long Counter { get; private set; }    // counter to reach next grade
        public long Condition { get; private set; }    // counter value needed to reach next grade (-1 if fully completed)
        public List<long> Timestamps { get; private set; }  // stored in ascending order of grades as seconds since epoch time
        
        public Achieve(int achieveId, int grade = 0, int counter = 0, List<long> timestamps = null)
        {
            this.Id = achieveId;
            this.CurrentGrade = grade;
            this.Counter = counter;
            this.Timestamps = timestamps ?? new List<long>();
            this.MaxGrade = AchieveMetadataStorage.GetNumGrades(this.Id);
            this.Condition =  AchieveMetadataStorage.GetCondition(this.Id, this.CurrentGrade);
        }

        public void AddCounter(int amount)
        {
            this.Counter += amount;
            // level up achievement if counter reached condition of next grade
            if ((this.Condition != 0) && (this.Counter >= this.Condition))
            {
                // level up but not fully completed
                if (this.CurrentGrade < this.MaxGrade)
                {
                    this.CurrentGrade++;
                    Condition = AchieveMetadataStorage.GetCondition(this.Id, this.CurrentGrade);
                }
                // level up and fully completed
                else
                    this.Condition = 0;

                Timestamps.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }
        }
    }
}
