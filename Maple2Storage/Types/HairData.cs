namespace Maple2Storage.Types;

public struct HairData
{
    public float BackLength { get; private set; }
    public float FrontLength { get; private set; }

    public CoordF BackPositionCoord { get; private set; }
    public CoordF BackPositionRotation { get; private set; }
    public CoordF FrontPositionCoord { get; private set; }
    public CoordF FrontPositionRotation { get; private set; }

    public HairData(float backLength, float frontLength, CoordF backPositionCoord, CoordF backPositionRotation, CoordF frontPositionCoord, CoordF frontPositionRotation)
    {
        BackLength = backLength;
        FrontLength = frontLength;
        BackPositionCoord = backPositionCoord;
        BackPositionRotation = backPositionRotation;
        FrontPositionCoord = frontPositionCoord;
        FrontPositionRotation = frontPositionRotation;
    }

    public override string ToString()
    {
        return $"HAIRDATA({BackLength:X2}, {FrontLength:X2}, {BackPositionCoord:X2}, {BackPositionRotation:X2}, {FrontPositionCoord:X2}, {FrontPositionRotation:X2})";
    }
}
