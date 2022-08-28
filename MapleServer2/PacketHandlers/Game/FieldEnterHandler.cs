using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class FieldEnterHandler : GamePacketHandler<FieldEnterHandler>
{
    public override RecvOp OpCode => RecvOp.ResponseFieldEnter;

    public override void Handle(GameSession session, PacketReader packet)
    {
        packet.ReadInt(); // ?

        // Liftable: 00 00 00 00 00
        // SendBreakable
        // Self
        Player player = session.Player;
        Account account = player.Account;
        session.EnterField(player);
        session.Send(StatPacket.SetStats(player.FieldPlayer));
        session.Send(StatPointPacket.WriteTotalStatPoints(player));
        session.Send(StatPointPacket.WriteTotalStatPoints(player)); // This packet is sent twice on GMS, not sure why 
        session.Send(StatPointPacket.WriteStatPointDistribution(player));
        session.Send(SkillPointPacket.ExtraSkillPoints(player));

        if (player.ActivePet is not null)
        {
            Pet pet = session.FieldManager.RequestPet(player.ActivePet, player.FieldPlayer);
            if (pet is not null)
            {
                player.FieldPlayer.ActivePet = pet;

                session.Send(PetPacket.LoadPetSettings(pet));
                session.Send(NoticePacket.Notice(SystemNotice.PetSummonOn, NoticeType.Chat | NoticeType.FastText));
            }
        }

        if (account.IsVip() && player.Session != null)
        {
            List<PremiumClubEffectMetadata> effectMetadatas = PremiumClubEffectMetadataStorage.GetBuffs();
            foreach (PremiumClubEffectMetadata effect in effectMetadatas)
            {
                player.FieldPlayer.AdditionalEffects.AddEffect(new(effect.EffectId, effect.EffectLevel)
                {
                    Duration = (int) (Math.Min(account.VIPExpiration - TimeInfo.Now(), 0x0FFFFFFF)),
                    IsBuff = true
                });
            }


            session.Send(PremiumClubPacket.ActivatePremium(player.FieldPlayer, account.VIPExpiration));
        }

        session.Send(EmotePacket.LoadEmotes(player));
        session.Send(MacroPacket.LoadControls(player.Macros));
        foreach (Wardrobe wardrobe in player.Wardrobes)
        {
            session.Send(WardrobePacket.Load(wardrobe));
        }
        session.Send(ChatStickerPacket.LoadChatSticker(player));

        session.Send(CubePacket.DecorationScore(account.Home));
        session.Send(CubePacket.LoadHome(player.FieldPlayer.ObjectId, player.Account.Home));
        session.Send(CubePacket.ReturnMap(player.ReturnMapId));
        session.Send(LapenshardPacket.Load(player.Inventory.LapenshardStorage));

        IEnumerable<Cube> cubes = session.FieldManager.State.Cubes.Values
            .Where(x => x.Value.PlotNumber == 1 && x.Value.Item.HousingCategory is ItemHousingCategory.Farming or ItemHousingCategory.Ranching)
            .Select(x => x.Value);
        foreach (Cube cube in cubes)
        {
            session.Send(FunctionCubePacket.UpdateFunctionCube(cube.CoordF.ToByte(), 2, 1));
        }

        if (player.Party is not null)
        {
            session.Send(PartyPacket.UpdatePlayer(player));
        }

        GlobalEvent globalEvent = GameServer.GlobalEventManager.GetCurrentEvent();
        if (globalEvent is not null && !MapMetadataStorage.MapIsInstancedOnly(player.MapId))
        {
            session.Send(GlobalPortalPacket.Notice(globalEvent));
        }

        FieldWar fieldWar = GameServer.FieldWarManager.CurrentFieldWar;
        if (fieldWar is not null && !MapMetadataStorage.MapIsInstancedOnly(player.MapId) && fieldWar.MapId != player.MapId)
        {
            session.Send(FieldWarPacket.LegionPopup(fieldWar.Id, fieldWar.EntryClosureTime.ToUnixTimeSeconds()));
        }

        session.Send(KeyTablePacket.SendHotbars(player.GameOptions));

        TrophyManager.OnMapEntered(player, player.MapId);

        QuestManager.OnMapEnter(player, player.MapId);


        MapProperty mapProperty = MapMetadataStorage.GetMapProperty(player.MapId);
        for (int i = 0; i < mapProperty.EnterBuffIds.Count; i++)
        {
            player.FieldPlayer.AdditionalEffects.AddEffect(new(mapProperty.EnterBuffIds[i], mapProperty.EnterBuffLevels[i]));
        }

        player.InitializeEffects();
    }
}
