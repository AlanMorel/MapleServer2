using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class TrophyMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public string[] Categories;
        [XmlElement(Order = 3)]
        public List<TrophyGradeMetadata> Grades;

        // Required for deserialization
        public TrophyMetadata()
        {
            Grades = new List<TrophyGradeMetadata>();
        }

        public TrophyMetadata(int id, List<TrophyGradeMetadata> grades)
        {
            Id = id;
            Grades = grades;
        }

        public override string ToString() =>
            $"TrophyMetadata(Id:{Id},Categories:{string.Join(",", Categories)},Grades:{string.Join(",", Grades)}";

        protected bool Equals(TrophyMetadata other)
        {
            return Id == other.Id
                && Grades.SequenceEqual(other.Grades);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((TrophyMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(TrophyMetadata left, TrophyMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TrophyMetadata left, TrophyMetadata right)
        {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class TrophyGradeMetadata
    {
        [XmlElement(Order = 1)]
        public int Grade;
        [XmlElement(Order = 2)]
        public long Condition;
        [XmlElement(Order = 3)]
        public byte RewardType;
        [XmlElement(Order = 4)]
        public int RewardCode;
        [XmlElement(Order = 5)]
        public int RewardValue;

        // Required for deserialization
        public TrophyGradeMetadata() { }

        public override string ToString() =>
            $"TrophyGradeMetadata(Grade:{Grade},Condition:{Condition},RewardType:{RewardType},RewardCode:{RewardCode},RewardValue:{RewardValue})";

        protected bool Equals(TrophyGradeMetadata other)
        {
            return Grade == other.Grade
                && Condition == other.Condition
                && RewardType == other.RewardType
                && RewardCode == other.RewardCode
                && RewardValue == other.RewardValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((TrophyGradeMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Grade, Condition, RewardType, RewardCode);
        }

        public static bool operator ==(TrophyGradeMetadata left, TrophyGradeMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TrophyGradeMetadata left, TrophyGradeMetadata right)
        {
            return !Equals(left, right);
        }
    }
}
