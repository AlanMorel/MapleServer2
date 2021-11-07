namespace Maple2Storage.Types;

public struct HatData
{
    // Unsure if these are the correct coords for each
    public CoordF XPosition { get; private set; }
    public CoordF YPosition { get; private set; }
    public CoordF ZPosition { get; private set; }
    public CoordF Rotation { get; private set; }
    public float Offset { get; private set; }

    public HatData(float offset, CoordF xPosition, CoordF yPosition, CoordF zPosition, CoordF rotation)
    {
        XPosition = xPosition;
        YPosition = yPosition;
        ZPosition = zPosition;
        Rotation = rotation;
        Offset = offset;
    }

    public override string ToString()
    {
        return $"HatData({Offset:X2}, {XPosition:X2}, {YPosition:X2}, {ZPosition:X2}, {Rotation:X2})";
    }
}
