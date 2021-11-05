namespace Maple2Storage.Enums;

public enum NpcAction : byte
{
    None = 0,
    Idle = 1,
    Walk = 2,
    Bore,
    Run,
    Jump = 6,
    Dead = 12,
    Hit = 13,
    Skill = 16,
    Spawn = 17,
    Stun = 18,
    Talk = 22,
    Regen = 23,
    Puppet = 35,
    SummonPortal = 56
}
