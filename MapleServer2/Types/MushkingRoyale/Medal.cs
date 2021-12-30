using Maple2Storage.Enums;
using MapleServer2.Database;

namespace MapleServer2.Types;

public class Medal
{
    public long Uid;
    public int EffectId;
    public bool IsEquipped;
    public long ExpirationTimeStamp;
    public Item Item;
    public long AccountId;
    public MedalSlot Slot;

    public Medal() { }

    public Medal(int effectId, Item item, long accountId, MedalSlot slot)
    {
        EffectId = effectId;
        Item = item;
        AccountId = accountId;
        Slot = slot;
        ExpirationTimeStamp = 2524608000; // setting all expirations to 2050. Unsure how it's determined.
        Uid = DatabaseManager.MushkingRoyaleMedals.Insert(this);
    }

    public Medal(long uid, int effectId, bool isEquipped, long expiration, Item item, MedalSlot slot, long accountId)
    {
        Uid = uid;
        EffectId = effectId;
        IsEquipped = isEquipped;
        ExpirationTimeStamp = expiration;
        Item = item;
        Slot = slot;
        AccountId = accountId;
    }
}
