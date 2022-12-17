using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types;

public class SetBonus
{
    public int EquipCount;

    public SetItemInfoMetadata SetInfo;
    public SetItemOptionMetadata Bonuses;

    public SetBonus(SetItemInfoMetadata optionInfo)
    {
        SetInfo = optionInfo;
        Bonuses = SetItemOptionMetadataStorage.GetMetadata(optionInfo.Id);
    }

    public bool HasItem(Item item)
    {
        return SetInfo.ItemIds.Contains(item.Id);
    }

    public static SetBonus? From(Item item)
    {
        SetItemInfoMetadata? info = SetItemInfoMetadataStorage.GetMetadataFromItem(item.Id);

        if (info is null)
        {
            return null;
        }

        return new SetBonus(info);
    }
}
