using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

public class Gemstone
{
    public int Id;
    public long OwnerId = 0;
    public string OwnerName = "";
    public bool IsLocked;
    public long UnlockTime;
    public ItemStats Stats;
    public ItemAdditionalEffectMetadata AdditionalEffects;
}

public class GemSocket
{
    public bool IsUnlocked;
    public Gemstone Gemstone;
}
