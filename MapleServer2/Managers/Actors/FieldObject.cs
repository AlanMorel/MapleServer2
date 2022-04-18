using Maple2Storage.Types;
using MapleServer2.Types;

namespace MapleServer2.Managers.Actors;

public class FieldObject<T> : IFieldObject<T>
{
    public int ObjectId { get; set; }
    public T Value { get; }

    public virtual CoordF Coord { get; set; }
    public CoordF Rotation { get; set; }
    public short LookDirection
    {
        get => (short) (Rotation.Z * 10);
        set => Rotation = CoordF.From(Rotation.X, Rotation.Y, value / 10);
    }

    public FieldObject(int objectId, T value)
    {
        ObjectId = objectId;
        Value = value;
    }
}
