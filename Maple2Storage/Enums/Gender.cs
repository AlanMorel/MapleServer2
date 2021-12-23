namespace Maple2Storage.Enums;

public enum Gender : byte
{
    Male = 0,
    Female = 1,
    Neutral = 2
}

[Flags]
public enum GenderFlag : byte
{
    Male = 1,
    Female = 2
}
