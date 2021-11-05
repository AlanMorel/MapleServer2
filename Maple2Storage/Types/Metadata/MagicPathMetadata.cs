using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MagicPathMetadata
{
    [XmlElement(Order = 1)]
    public readonly long Id;
    [XmlElement(Order = 2)]
    public readonly List<MagicPathMove> MagicPathMoves = new();

    public MagicPathMetadata()
    {
        MagicPathMoves = new();
    }

    public MagicPathMetadata(long id, List<MagicPathMove> magicPathMoves)
    {
        Id = id;
        MagicPathMoves = magicPathMoves;
    }

    public override string ToString()
    {
        return $"ID:{Id} {string.Join(",", MagicPathMoves)}";
    }
}
[XmlType]
public class MagicPathMove
{
    [XmlElement(Order = 1)]
    public readonly int Rotation;
    [XmlElement(Order = 2)]
    public readonly CoordF FireOffsetPosition;
    [XmlElement(Order = 3)]
    public readonly CoordF Direction;
    [XmlElement(Order = 4)]
    public readonly CoordF ControlValue0;
    [XmlElement(Order = 5)]
    public readonly CoordF ControlValue1;
    [XmlElement(Order = 6)]
    public readonly bool IgnoreAdjust;

    public MagicPathMove() { }

    public MagicPathMove(int rotation, CoordF fireOffsetPosition, CoordF direction, CoordF controlValue0, CoordF controlValue1, bool ignoreAdjust)
    {
        Rotation = rotation;
        FireOffsetPosition = fireOffsetPosition;
        Direction = direction;
        ControlValue0 = controlValue0;
        ControlValue1 = controlValue1;
        IgnoreAdjust = ignoreAdjust;
    }

    public override string ToString()
    {
        return $"Rotation:{Rotation}, FireOffsetPos:{FireOffsetPosition}," +
            $"Direction:{Direction}, ControlValue0:{ControlValue0}, ControlValue1:{ControlValue1}, IgnoreAdjust:{IgnoreAdjust}";
    }
}
