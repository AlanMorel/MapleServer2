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
        public int AttackType;
        [XmlElement(Order = 4)]
        public int Type;
        [XmlElement(Order = 5)]
        public int SubType;
        [XmlElement(Order = 6)]
        public int RangeType;
        [XmlElement(Order = 7)]
        public int Element;
        [XmlElement(Order = 8)]
        public byte DefaultSkill;
        [XmlElement(Order = 9)]
        public readonly List<SkillLevel> SkillLevel;

        public SkillMetadata()
        {
            this.SkillLevel = new List<SkillLevel>();
        }

        public SkillMetadata(int skillId)
        {
            this.SkillId = skillId;
            this.SkillLevel = new List<SkillLevel>();
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
                && AttackType.Equals(other.AttackType)
                && Type.Equals(other.Type)
                && SubType.Equals(other.SubType)
                && RangeType.Equals(other.RangeType)
                && Element.Equals(other.Element)
                && DefaultSkill.Equals(other.DefaultSkill)
                && SkillLevel.Equals(other.SkillLevel);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SkillId, Feature, AttackType, Type, SubType, RangeType, Element, SkillLevel);
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
            $"Skill:(Id:{SkillId},Feature:{Feature},AttackType:{AttackType},Type:{Type},SubType:{SubType},Range:{RangeType},Element:{Element},Default:{DefaultSkill},SkillLevel:{string.Join(",", SkillLevel)}";
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
        [XmlElement(Order = 6)]
        public readonly List<SkillMotion> SkillMotion;

        // Required for deserialization
        public SkillLevel()
        {
            this.SkillMotion = new List<SkillMotion>();
        }

        public SkillLevel(int _Level, int _Spirit, int _UpgradeLevel, int[] _UpgradeSkillId, int[] _UpgradeSkillLevel, List<SkillMotion> motion)
        {
            this.Level = _Level;
            this.Spirit = _Spirit;
            this.UpgradeLevel = _UpgradeLevel;
            this.UpgradeSkillId = _UpgradeSkillId;
            this.UpgradeSkillLevel = _UpgradeSkillLevel;
            this.SkillMotion = motion;
        }

        protected bool Equals(SkillLevel other)
        {
            return Level == other.Level
                && Spirit.Equals(other.Spirit)
                && UpgradeLevel.Equals(other.UpgradeLevel)
                && UpgradeSkillId.Equals(other.UpgradeSkillId)
                && UpgradeSkillLevel.Equals(other.UpgradeSkillLevel)
                && SkillMotion.Equals(other.SkillMotion);
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
            return HashCode.Combine(Level, Spirit, UpgradeLevel, UpgradeSkillId, UpgradeSkillLevel, SkillMotion);
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
            $"SkillLevel(Level:{Level},Spirit:{Spirit},UpgradeLevel:{UpgradeLevel},SkillRequired:{UpgradeSkillId},SkillLevelRequired:{UpgradeSkillLevel},Motions:{string.Join(",", SkillMotion)})";
    }

    [XmlType]
    public class SkillMotion
    {
        [XmlElement(Order = 1)]
        public string SequenceName;
        [XmlElement(Order = 2)]
        public string MotionEffect;
        [XmlElement(Order = 3)]
        public string StrTagEffects;

        public SkillMotion() { }

        public SkillMotion(string _SequenceName, string _MotionEffect, string _StrTagEffects)
        {
            this.SequenceName = _SequenceName;
            this.MotionEffect = _MotionEffect;
            this.StrTagEffects = _StrTagEffects;
        }

        protected bool Equals(SkillMotion other)
        {
            return SequenceName == other.SequenceName && MotionEffect.Equals(other.MotionEffect) && StrTagEffects.Equals(other.StrTagEffects);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((SkillMotion) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SequenceName, MotionEffect, StrTagEffects);
        }

        public static bool operator ==(SkillMotion left, SkillMotion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SkillMotion left, SkillMotion right)
        {
            return !Equals(left, right);
        }

        public override string ToString() =>
            $"SkillMotion(Name:{SequenceName},MotionEffect:{MotionEffect},TagEffect:{StrTagEffects})";
    }
}
