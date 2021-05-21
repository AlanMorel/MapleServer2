using System;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class MasteryFactorMetadata
    {
        [XmlElement(Order = 1)]
        public int Differential;
        [XmlElement(Order = 2)]
        public int Factor;

        // Required for deserialization
        public MasteryFactorMetadata()
        {
        }

        public MasteryFactorMetadata(int differential, int factor)
        {
            Differential = differential;
            Factor = factor;
        }

        public override string ToString() =>
            $"MasteryFactorMetadata(Differential:{Differential},Factor:{Factor})";

        protected bool Equals(MasteryFactorMetadata other)
        {
            return Differential == other.Differential
                   && Factor == other.Factor;
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

            return Equals((MasteryFactorMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Differential, Factor);
        }

        public static bool operator ==(MasteryFactorMetadata left, MasteryFactorMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MasteryFactorMetadata left, MasteryFactorMetadata right)
        {
            return !Equals(left, right);
        }
    }
}
