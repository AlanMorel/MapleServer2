using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class MasteryMetadata
    {
        [XmlElement(Order = 1)]
        public int Type;
        [XmlElement(Order = 2)]
        public List<MasteryGrade> Grades;

        // Required for deserialization
        public MasteryMetadata()
        {
            Grades = new List<MasteryGrade>();
        }

        public MasteryMetadata(int type, List<MasteryGrade> grades)
        {
            Type = type;
            Grades = grades;
        }

        public override string ToString() =>
            $"MasteryMetadata(Type:{Type},Grades:{string.Join(",", Grades)})";

        protected bool Equals(MasteryMetadata other)
        {
            return Type == other.Type
                   && Grades.SequenceEqual(other.Grades);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((MasteryMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type);
        }

        public static bool operator ==(MasteryMetadata left, MasteryMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MasteryMetadata left, MasteryMetadata right)
        {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class MasteryGrade
    {
        [XmlElement(Order = 1)]
        public int Grade;
        [XmlElement(Order = 2)]
        public long Value;
        [XmlElement(Order = 3)]
        public int RewardJobItemID;
        [XmlElement(Order = 4)]
        public int RewardJobItemRank;
        [XmlElement(Order = 5)]
        public int RewardJobItemCount;
        [XmlElement(Order = 6)]
        public string Feature;

        // Required for deserialization
        public MasteryGrade() { }

        public override string ToString() =>
            $"MasteryGradeMetadata(Grade:{Grade},Value:{Value},RewardJobItemID:{RewardJobItemID},RewardJobItemRank:{RewardJobItemRank},RewardJobItemCount:{RewardJobItemCount},Feature:{Feature})";

        protected bool Equals(MasteryGrade other)
        {
            return Grade == other.Grade
                   && Value == other.Value
                   && RewardJobItemID == other.RewardJobItemID
                   && RewardJobItemRank == other.RewardJobItemRank
                   && RewardJobItemCount == other.RewardJobItemCount
                   && Feature == other.Feature;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((MasteryGrade) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Grade, Value, RewardJobItemID, RewardJobItemRank, RewardJobItemCount, Feature);
        }

        public static bool operator ==(MasteryGrade left, MasteryGrade right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MasteryGrade left, MasteryGrade right)
        {
            return !Equals(left, right);
        }
    }
}
