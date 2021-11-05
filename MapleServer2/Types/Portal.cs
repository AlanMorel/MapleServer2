using Maple2Storage.Enums;
using Maple2Storage.Types;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class Portal
{
    public int Id;
    public bool IsVisible;
    public bool IsEnabled;
    public bool IsMinimapVisible;
    public CoordF Rotation;
    public int TargetMapId;
    public int TargetPortalId;
    public int Duration;
    public string Effect;
    public string Host;
    public bool IsPassEnabled;
    public string Passcode;
    public PortalTypes PortalType;
    public UGCPortalMethod UGCPortalMethod;
    public long TargetHomeAccountId;

    public Portal(int id)
    {
        Id = id;
    }

    public void Update(bool visible, bool enabled, bool minimapVisible)
    {
        IsVisible = visible;
        IsEnabled = enabled;
        IsMinimapVisible = minimapVisible;
    }
}
