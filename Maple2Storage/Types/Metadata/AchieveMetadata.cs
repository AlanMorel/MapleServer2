using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class AchieveMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public List<AchieveGradeMetadata> Grades;

        // Required for deserialization
        public AchieveMetadata()
        {
            Grades = new List<AchieveGradeMetadata>();
        }

        public AchieveMetadata(int id, List<AchieveGradeMetadata> grades)
        {
            Id = id;
            Grades = grades;
        }

        public override string ToString() =>
            $"AchieveMetadata(Id:{Id},Grades:{string.Join(",", Grades)}";

        protected bool Equals(AchieveMetadata other)
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

            return Equals((AchieveMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(AchieveMetadata left, AchieveMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AchieveMetadata left, AchieveMetadata right)
        {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class AchieveGradeMetadata
    {
        [XmlElement(Order = 1)]
        public int Grade;
        [XmlElement(Order = 2)]
        public long Condition;
        [XmlElement(Order = 3)]
        public RewardType RewardType;
        [XmlElement(Order = 4)]
        public int RewardCode;

        // Required for deserialization
        public AchieveGradeMetadata() { }

        public override string ToString() =>
            $"AchieveGradeMetadata(Grade:{Grade},Condition:{Condition},RewardType:{RewardType},RewardCode:{RewardCode})";

        protected bool Equals(AchieveGradeMetadata other)
        {
            return Grade == other.Grade
                && Condition == other.Condition
                && RewardType == other.RewardType
                && RewardCode == other.RewardCode;
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

            return Equals((AchieveGradeMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Grade, Condition, RewardType, RewardCode);
        }

        public static bool operator ==(AchieveGradeMetadata left, AchieveGradeMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AchieveGradeMetadata left, AchieveGradeMetadata right)
        {
            return !Equals(left, right);
        }
    }
}
