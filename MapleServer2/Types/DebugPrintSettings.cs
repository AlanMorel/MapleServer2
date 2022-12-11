using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapleServer2.Types;

public class DebugPrintSettings
{
    public int TargetsToPrint = 0;
    public bool PrintOwnEffects = false;
    public bool PrintCastedEffects = false;
    public bool PrintEffectsFromOthers = false;
}
