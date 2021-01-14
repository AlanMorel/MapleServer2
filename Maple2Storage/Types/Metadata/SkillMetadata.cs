using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class SkillMetadata
    {
        [XmlElement(Order = 1)]
        public int SkillId;
        [XmlElement(Order = 2)]
        public string Feature = "";
        [XmlElement(Order = 3)]
        public int Type;
        [XmlElement(Order = 4)]
        public int SubType;
        [XmlElement(Order = 5)]
        public byte DefaultSkill;
        [XmlElement(Order = 6)]
        public SkillLevel SkillLevel;
        [XmlElement(Order = 7)]
        public byte Learned = 0;

        public SkillMetadata()
        {
            this.SkillLevel = new SkillLevel();
        }

        public SkillMetadata(int skillId)
        {
            this.SkillId = skillId;
            this.SkillLevel = new SkillLevel();
        }

        public SkillMetadata(int skillId, byte learned, SkillLevel skillLevel)
        {
            this.SkillId = skillId;
            this.SkillLevel = skillLevel;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((SkillMetadata) obj);
        }

        protected bool Equals(SkillMetadata other)
        {
            return SkillId == other.SkillId
                && Feature.Equals(other.Feature)
                && Type.Equals(other.Type)
                && SubType.Equals(other.SubType)
                && DefaultSkill.Equals(other.DefaultSkill)
                && SkillLevel.Equals(other.SkillLevel)
                && Learned.Equals(other.Learned);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SkillId, Feature, Type, SubType, SkillLevel, Learned);
        }

        public static bool operator ==(SkillMetadata left, SkillMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SkillMetadata left, SkillMetadata right)
        {
            return !Equals(left, right);
        }

        public override string ToString() =>
            $"Skill:(Id:{SkillId},Feature:{Feature},Type:{Type},SubType:{SubType},Default:{DefaultSkill},SkillLevel:{string.Join(",", SkillLevel)},Learned:{Learned})";
    }

    [XmlType]
    public class SkillLevel
    {
        [XmlElement(Order = 1)]
        public int Level;
        [XmlElement(Order = 2)]
        public int Spirit;
        [XmlElement(Order = 3)]
        public int UpgradeLevel;
        [XmlElement(Order = 4)]
        public int[] UpgradeSkillId;
        [XmlElement(Order = 5)]
        public int[] UpgradeSkillLevel;

        // Required for deserialization
        public SkillLevel()
        {

        }

        public SkillLevel(int _Level, int _Spirit, int _UpgradeLevel, int[] _UpgradeSkillId, int[] _UpgradeSkillLevel)
        {
            this.Level = _Level;
            this.Spirit = _Spirit;
            this.UpgradeLevel = _UpgradeLevel;
            this.UpgradeSkillId = _UpgradeSkillId;
            this.UpgradeSkillLevel = _UpgradeSkillLevel;
        }

        protected bool Equals(SkillLevel other)
        {
            return Level == other.Level
                && Spirit.Equals(other.Spirit)
                && UpgradeLevel.Equals(other.UpgradeLevel)
                && UpgradeSkillId.Equals(other.UpgradeSkillId)
                && UpgradeSkillLevel.Equals(other.UpgradeSkillLevel);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((SkillLevel) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level, Spirit, UpgradeLevel, UpgradeSkillId, UpgradeSkillLevel);
        }

        public static bool operator ==(SkillLevel left, SkillLevel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SkillLevel left, SkillLevel right)
        {
            return !Equals(left, right);
        }

        public override string ToString() =>
            $"SkillLevel(Level:{Level},Spirit:{Spirit},UpgradeLevel:{UpgradeLevel},SkillRequired:{UpgradeSkillId},SkillLevelRequired:{UpgradeSkillLevel})";
    }
}
