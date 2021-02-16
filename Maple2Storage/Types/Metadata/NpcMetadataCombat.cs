using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class NpcMetadataCombat
    {
        [XmlElement]
        public uint CombatAbandonTick;  // 0, or 999999
        [XmlElement]
        public uint CombatAbandonImpossibleTick;  // 0, or 999999
        [XmlElement]
        public bool CanIgnoreExtendedLifetime;  // "true" or "false" in xml. Coerced to bool here.
        [XmlElement]
        public bool CanShowHiddenTarget;  // "true" or "false' in xml. Coerced to bool here.

        public NpcMetadataCombat()
        {

        }
    }
}
