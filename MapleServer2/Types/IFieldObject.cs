using Maple2Storage.Types;

namespace MapleServer2.Types;

public interface IFieldObject
{
    public int ObjectId { get; }
    public CoordF Coord { get; set; }
    public CoordF Rotation { get; set; }
    public short LookDirection { get; set; }
}

public interface IFieldObject<out T> : IFieldObject
{
    public T Value { get; }
}
