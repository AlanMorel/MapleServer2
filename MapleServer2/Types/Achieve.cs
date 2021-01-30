using System;
using System.Collections.Generic;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Achieve
    {
        public Player Owner { get; private set; }
        public int Id { get; private set; }
        public int Grade {get; private set; }   // grade trying to achieve
        public int NumGrades {get; private set; }
        public long Counter { get; private set; }    // counter to reach next grade
        public long Max { get; private set; }    // counter value needed to reach next grade (-1 if grade is max)
        public List<long> Timestamps { get; private set; }  // stored in ascending order of grades as seconds since epoch time
        
        public Achieve(int achieveId, int grade = 0, int counter = 0, List<long> timestamps = null)
        {
            this.Id = achieveId;
            this.Grade = grade;
            this.Counter = counter;
            this.Timestamps = timestamps ?? new List<long>();
            this.NumGrades = AchieveMetadataStorage.GetNumGrades(this.Id);
            this.Max =  AchieveMetadataStorage.GetCondition(this.Id, this.Grade);
        }

        public void AddCounter(int amount)
        {
            this.Counter += amount;
            // level up achievement if counter reached condition of next grade
            if (this.Counter >= this.Max)
            {
                if (this.Grade < this.NumGrades)
                    this.Grade++;
                Max = AchieveMetadataStorage.GetCondition(this.Id, this.Grade);
                Timestamps.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }
        }
    }
}
