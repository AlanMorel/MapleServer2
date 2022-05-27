using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

public class EnchantScrollHandler : GamePacketHandler<EnchantScrollHandler>
{
    public override RecvOp OpCode => RecvOp.EnchantScroll;

    private enum EnchantScrollMode : byte
    {
        AddItem = 0x1,
        UseScroll = 0x2,
    }

    private enum EnchantScrollError : short
    {
        None = 0x0,
        ItemsNoLongerValid = 0x1,
        IneligibleItem = 0x2,
        UnstableItemCannotBeEnchanted = 0x3,
        InsufficientItemLevel = 0x4,
        GearCannotBeEnchanted = 0x5,
        CannotBeEnchantedDueToRarity = 0x6,
        GearRarityIsHigher = 0x7,
        UnstableItemCannotBeRestored = 0x8,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        EnchantScrollMode mode = (EnchantScrollMode) packet.ReadByte();

        long scrollUid = packet.ReadLong();
        long equipUid = packet.ReadLong();

        Player player = session.Player;
        if (!player.Inventory.HasItem(equipUid) && !player.Inventory.HasItem(scrollUid))
        {
            session.Send(EnchantScrollPacket.UseScroll((short) EnchantScrollError.ItemsNoLongerValid));
            return;
        }

        Item scroll = player.Inventory.GetByUid(scrollUid);
        Item equip = player.Inventory.GetByUid(equipUid);

        EnchantScrollMetadata metadata = EnchantScrollMetadataStorage.GetMetadata(scroll.Function.Id);
        if (metadata is null)
        {
            return;
        }

        if (!metadata.Rarities.Contains(equip.Rarity))
        {
            session.Send(EnchantScrollPacket.UseScroll((short) EnchantScrollError.CannotBeEnchantedDueToRarity));
            return;
        }

        if (metadata.MinLevel > equip.Level || metadata.MaxLevel < equip.Level)
        {
            session.Send(EnchantScrollPacket.UseScroll((short) EnchantScrollError.InsufficientItemLevel));
            return;
        }

        if (!metadata.ItemTypes.Contains(equip.Type))
        {
            session.Send(EnchantScrollPacket.UseScroll((short) EnchantScrollError.IneligibleItem));
            return;
        }

        if (equip.EnchantLevel >= metadata.EnchantLevels.Last())
        {
            session.Send(EnchantScrollPacket.UseScroll((short) EnchantScrollError.GearCannotBeEnchanted));
            return;
        }

        int enchantLevelIndex = Random.Shared.Next(metadata.EnchantLevels.Count);
        Dictionary<StatAttribute, ItemStat> enchantStats = EnchantHelper.GetEnchantStats(metadata.EnchantLevels[enchantLevelIndex], equip.Type, equip.Level);

        switch (mode)
        {
            case EnchantScrollMode.AddItem:
                session.Send(EnchantScrollPacket.AddItem(equipUid, enchantStats));
                break;
            case EnchantScrollMode.UseScroll:
                HandleUseScroll(session, equip, scroll, enchantStats, metadata.EnchantLevels[enchantLevelIndex], metadata.Id);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleUseScroll(GameSession session, Item equip, Item scroll, Dictionary<StatAttribute, ItemStat> enchantStats, int enchantLevel, int scrollId)
    {
        Script script = ScriptLoader.GetScript("Functions/ItemEnchantScroll/getSuccessRate");
        float successRate = (float) script.RunFunction("getSuccessRate", scrollId).Number;

        int randomValue = Random.Shared.Next(0, 10000 + 1);
        bool scrollSuccess = successRate * 10000 >= randomValue;

        session.Player.Inventory.ConsumeItem(session, scroll.Uid, 1);

        if (scrollSuccess)
        {
            equip.EnchantLevel = enchantLevel;
            equip.EnchantExp = 0;
            equip.Stats.Enchants = enchantStats;
            session.Send(EnchantScrollPacket.UseScroll((short) EnchantScrollError.None, equip));
        }
    }
}
