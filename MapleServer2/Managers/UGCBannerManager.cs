using System.Collections.Concurrent;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Types;
using Serilog;

namespace MapleServer2.Managers;

public class UGCBannerManager
{
    private readonly ConcurrentDictionary<long, UGCBanner> UGCBanners = new();

    public UGCBannerManager()
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        Dictionary<int, List<int>> bannerIds = MapEntityMetadataStorage.GetAdBannerIds();
        foreach ((int mapId, List<int> ids) in bannerIds)
        {
            foreach (int id in ids)
            {
                UGCBanner banner = new(id, mapId);

                DeleteOldBannerSlots(banner, utcNow);
                ActivateBannerSlots(banner, utcNow);

                UGCBanners.TryAdd(banner.Id, banner);
            }
        }
    }

    public void AddBanner(UGCBanner banner)
    {
        UGCBanners.TryAdd(banner.Id, banner);
    }

    public UGCBanner GetBanner(long id)
    {
        return UGCBanners.TryGetValue(id, out UGCBanner banner) ? banner : null;
    }

    public UGCBanner UpdateBannerSlots(UGC ugc)
    {
        UGCBanner ugcBanner = UGCBanners.Values.FirstOrDefault(x => x.Slots.Any(y => y.UGC.Uid == ugc.Uid));
        if (ugcBanner is null)
        {
            return null;
        }

        foreach (BannerSlot slot in ugcBanner.Slots.Where(slot => slot.UGC.Uid == ugc.Uid))
        {
            slot.UGC.Url = ugc.Url;
        }

        return ugcBanner;
    }

    public void UpdateBanner(UGCBanner banner)
    {
        if (!UGCBanners.ContainsKey(banner.Id))
        {
            Log.Logger.ForContext<UGCBannerManager>().Warning("Tried to update a banner that doesn't exist");
            return;
        }

        UGCBanners[banner.Id] = banner;
    }

    public List<UGCBanner> GetBanners(int mapId)
    {
        return UGCBanners.Values.Where(x => x.MapId == mapId).ToList();
    }

    public void UGCBannerLoop(FieldManager field)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
        foreach (UGCBanner ugcBanner in UGCBanners.Values)
        {
            DeleteOldBannerSlots(ugcBanner, dateTimeOffset);

            if (!ActivateBannerSlots(ugcBanner, dateTimeOffset))
            {
                continue;
            }

            field.BroadcastPacket(UGCPacket.ActivateBanner(ugcBanner));
        }
    }

    private static void DeleteOldBannerSlots(UGCBanner ugcBanner, DateTimeOffset dateTimeOffset)
    {
        List<BannerSlot> oldBannerSlots = new();
        foreach (BannerSlot bannerSlot in ugcBanner.Slots)
        {
            // check if the banner is expired
            DateTimeOffset expireTimeStamp = dateTimeOffset.Subtract(TimeSpan.FromHours(4));
            if (bannerSlot.ActivateTime >= expireTimeStamp)
            {
                continue;
            }

            oldBannerSlots.Add(bannerSlot);
            DatabaseManager.BannerSlot.Delete(bannerSlot.Id);
        }

        ugcBanner.Slots.RemoveAll(x => oldBannerSlots.Contains(x));
    }

    private static bool ActivateBannerSlots(UGCBanner ugcBanner, DateTimeOffset dateTimeOffset)
    {
        BannerSlot slot = ugcBanner.Slots.FirstOrDefault(x => x.ActivateTime.Day == dateTimeOffset.Day && x.ActivateTime.Hour == dateTimeOffset.Hour);

        if (slot is null)
        {
            return false;
        }

        slot.Active = true;
        DatabaseManager.BannerSlot.UpdateActive(slot.Id, slot.Active);
        return true;
    }
}
