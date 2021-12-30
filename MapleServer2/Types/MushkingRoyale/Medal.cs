using Maple2Storage.Enums;
using MapleServer2.Database;

namespace MapleServer2.Types;

public class Medal
{
    public long Uid;
    public int Id;
    public bool IsEquipped;
    public long ExpirationTimeStamp;
    public Item Item;
    public long AccountId;
    public MedalSlot Slot;

    public Medal() { }

    public Medal(int id, Item item, long accountId, MedalSlot slot)
    {
        Id = id;
        Item = item;
        AccountId = accountId;
        Slot = slot;
        ExpirationTimeStamp = 2524608000; // setting all expirations to 2050. Unsure how it's determined.
        Uid = DatabaseManager.MushkingRoyaleMedals.Insert(this);
    }

    public Medal(long uid, int id, bool isEquipped, long expiration, Item item, MedalSlot slot, long accountId)
    {
        Uid = uid;
        Id = id;
        IsEquipped = isEquipped;
        ExpirationTimeStamp = expiration;
        Item = item;
        Slot = slot;
        AccountId = accountId;
    }
}
