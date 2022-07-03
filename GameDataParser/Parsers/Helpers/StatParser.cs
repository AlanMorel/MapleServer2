using System.Xml;
using Maple2Storage.Types;

namespace GameDataParser.Parsers.Helpers;

public static class StatParser
{
    public static XmlStats ParseStats(XmlAttributeCollection collection)
    {
        int ParseAttribute(string name)
        {
            int.TryParse(collection[name]?.Value, out int value);
            return value;
        }

        // MUST be in ORDER
        XmlStats xmlStats = new()
        {
            Str = new(ParseAttribute("str")),
            Dex = new(ParseAttribute("dex")),
            Int = new(ParseAttribute("int")),
            Luk = new(ParseAttribute("luk")),
            Hp = new(ParseAttribute("hp")),
            HpRegen = new(ParseAttribute("hp_rgp")),
            HpInterval = new(ParseAttribute("hp_inv")),
            Sp = new(ParseAttribute("sp")),
            SpRegen = new(ParseAttribute("sp_rgp")),
            SpInterval = new(ParseAttribute("sp_inv")),
            Ep = new(ParseAttribute("ep")),
            EpRegen = new(ParseAttribute("ep_rgp")),
            EpInterval = new(ParseAttribute("ep_inv")),
            AtkSpd = new(ParseAttribute("asp")),
            MoveSpd = new(ParseAttribute("msp")),
            Accuracy = new(ParseAttribute("atp")),
            Evasion = new(ParseAttribute("evp")),
            CritRate = new(ParseAttribute("cap")),
            CritDamage = new(ParseAttribute("cad")),
            CritResist = new(ParseAttribute("car")),
            Defense = new(ParseAttribute("ndd")),
            Guard = new(ParseAttribute("abp")),
            JumpHeight = new(ParseAttribute("jmp")),
            PhysAtk = new(ParseAttribute("pap")),
            MagAtk = new(ParseAttribute("map")),
            PhysRes = new(ParseAttribute("par")),
            MagRes = new(ParseAttribute("mar")),
            MinAtk = new(ParseAttribute("wapmin")),
            MaxAtk = new(ParseAttribute("wapmax")),
            Damage = new(ParseAttribute("dmg")),
            Pierce = new(ParseAttribute("pen")),
            MountSpeed = new(ParseAttribute("rmsp"))
        };

        return xmlStats;
    }
}
