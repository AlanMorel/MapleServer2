using Maple2Storage.Types;

namespace MapleServer2.Types;

public interface IFieldObject<out T>
{
    public int ObjectId { get; }
    public T Value { get; }

    public CoordF Coord { get; set; }

    public CoordF Rotation { get; set; }
}
