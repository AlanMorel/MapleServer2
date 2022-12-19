using System.Diagnostics;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class BeautyHelper
{
    public static void ChangeHair(GameSession session, int hairId, out Item? previousHair, out Item? newHair)
    {
        previousHair = null;
        newHair = null;

        //Grab a preset hair and length of hair
        ItemCustomizeMetadata? customize = ItemMetadataStorage.GetMetadata(hairId)?.Customize;
        if (customize is null)
        {
            return;
        }

        int indexPreset = Random.Shared.Next(customize.HairPresets.Count);
        HairPresets chosenPreset = customize.HairPresets[indexPreset];

        //Grab random front hair length
        double chosenFrontLength = Random.Shared.NextDouble() *
            (customize.HairPresets[indexPreset].MaxScale - customize.HairPresets[indexPreset].MinScale) + customize.HairPresets[indexPreset].MinScale;

        //Grab random back hair length
        double chosenBackLength = Random.Shared.NextDouble() *
            (customize.HairPresets[indexPreset].MaxScale - customize.HairPresets[indexPreset].MinScale) + customize.HairPresets[indexPreset].MinScale;

        // Grab random preset color
        // pick from palette 2. Seems like it's the correct palette for basic hair colors
        ColorPaletteMetadata? palette = ColorPaletteMetadataStorage.GetMetadata(2);

        if (palette == null)
        {
            return;
        }

        int indexColor = Random.Shared.Next(palette.DefaultColors.Count);
        MixedColor color = palette.DefaultColors[indexColor];

        newHair = new(hairId)
        {
            Color = EquipColor.Argb(color, indexColor, palette.PaletteId),
            HairData = new((float) chosenBackLength, (float) chosenFrontLength, chosenPreset.BackPositionCoord, chosenPreset.BackPositionRotation,
                chosenPreset.FrontPositionCoord, chosenPreset.FrontPositionRotation),
            IsEquipped = true,
            OwnerCharacterId = session.Player.CharacterId,
            OwnerCharacterName = session.Player.Name
        };

        Dictionary<ItemSlot, Item> cosmetics = session.Player.Inventory.Cosmetics;

        //Remove old hair
        if (cosmetics.Remove(ItemSlot.HR, out previousHair))
        {
            previousHair.Slot = -1;
            session.Player.HairInventory.RandomHair = previousHair; // store the previous hair
            DatabaseManager.Items.Delete(previousHair.Uid);

            Debug.Assert(session.Player.FieldPlayer != null, "session.Player.FieldPlayer != null");
            session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, previousHair));
        }

        cosmetics[ItemSlot.HR] = newHair;
    }

    public static void ChangeFace(GameSession session, int faceId, out Item? previousFace, out Item? newFace)
    {
        // Grab random preset color
        // pick from palette 2. Seems like it's the correct palette for basic face colors
        ColorPaletteMetadata? palette = ColorPaletteMetadataStorage.GetMetadata(1);
        if (palette is null)
        {
            previousFace = null!;
            newFace = null!;
            return;
        }

        int indexColor = Random.Shared.Next(palette.DefaultColors.Count);
        MixedColor color = palette.DefaultColors[indexColor];

        newFace = new(faceId)
        {
            Color = EquipColor.Argb(color, indexColor, palette.PaletteId),
            IsEquipped = true,
            OwnerCharacterId = session.Player.CharacterId,
            OwnerCharacterName = session.Player.Name
        };
        Dictionary<ItemSlot, Item> cosmetics = session.Player.Inventory.Cosmetics;

        //Remove old face
        if (cosmetics.Remove(ItemSlot.FA, out previousFace))
        {
            previousFace.Slot = -1;
            DatabaseManager.Items.Delete(previousFace.Uid);
            Debug.Assert(session.Player.FieldPlayer != null, "session.Player.FieldPlayer != null");
            session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, previousFace));
        }

        cosmetics[ItemSlot.FA] = newFace;
    }
}
