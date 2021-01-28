using System;
using System.Collections.Generic;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Achieve
    {
        public Player Owner { get; private set; }
        public int Id { get; private set; }
        public int Grade {get; private set; }   // currently achieved grade (0 means not completed at all)
        public int NumGrades {get; private set; }
        public long Counter { get; private set; }    // counter to reach next grade
        public long Max { get; private set; }    // counter value needed to reach next grade (-1 if grade is max)
        public List<long> Timestamps { get; private set; }  // stored in order of grades as seconds since epoch time
        
        public Achieve(int achieveId, int grade = 0, int counter = 0, List<long> timestamps = null)
        {
            this.Id = achieveId;
            this.Grade = grade;
            this.Counter = counter;
            this.Timestamps = timestamps ?? new List<long>();
            this.NumGrades = AchieveMetadataStorage.GetNumGrades(this.Id);
            this.Max =  AchieveMetadataStorage.GetCondition(this.Id, this.Grade+1);
        }
        public void AddCounter(int amount)
        {
            Counter += amount;
            // keep leveling up achievement if there is still a next grade and condition is met
            while ((this.Grade < this.NumGrades) && (this.Counter > this.Max))
            {
                this.Grade++;
                Max = AchieveMetadataStorage.GetCondition(this.Id, this.Grade+1);
                Timestamps.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            }
        }
    }
}
