using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

public class ItemUseHandler : GamePacketHandler<ItemUseHandler>
{
    public override RecvOp OpCode => RecvOp.RequestItemUse;

    public override void Handle(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        if (!session.Player.Inventory.HasItem(itemUid))
        {
            return;
        }

        Item item = session.Player.Inventory.GetByUid(itemUid);
        if (item is null || !item.CanUse(session))
        {
            return;
        }

        switch (item.Function.Name)
        {
            case "CallAirTaxi":
                HandleCallAirTaxi(session, packet, item);
                break;
            case "ChatEmoticonAdd":
                HandleChatEmoticonAdd(session, item);
                break;
            case "SelectItemBox": // Item box selection reward
                HandleSelectItemBox(session, packet, item);
                break;
            case "OpenItemBox": // Item box random/fixed reward
                HandleOpenItemBox(session, packet, item);
                break;
            case "OpenMassive": // Player hosted mini game
                HandleOpenMassive(session, packet, item);
                break;
            case "LevelPotion":
                HandleLevelPotion(session, item);
                break;
            case "TitleScroll":
                HandleTitleScroll(session, item);
                break;
            case "OpenInstrument":
                HandleOpenInstrument(item);
                break;
            case "VIPCoupon":
                HandleVIPCoupon(session, item);
                break;
            case "StoryBook":
                HandleStoryBook(session, item);
                break;
            case "HongBao":
                HandleHongBao(session, item);
                break;
            case "ItemRemakeScroll":
                HandleItemRemakeScroll(session, itemUid);
                break;
            case "OpenGachaBox": // Gacha capsules
                HandleOpenGachaBox(session, packet, item);
                break;
            case "OpenCoupleEffectBox": // Buddy badges
                HandleOpenCoupleEffectBox(session, packet, item);
                break;
            case "PetExtraction": // Pet skin scroll
                HandlePetExtraction(session, packet, item);
                break;
            case "InstallBillBoard": // ad balloons
                HandleInstallBillBoard(session, packet, item);
                break;
            case "ExpendCharacterSlot":
                HandleExpandCharacterSlot(session, item);
                break;
            case "ItemChangeBeauty": // special beauty vouchers
                HandleBeautyVoucher(session, item);
                break;
            case "ItemRePackingScroll":
                HandleRepackingScroll(session, item);
                break;
            case "ChangeCharName":
                HandleNameVoucher(session, packet, item);
                break;
            case "ChangeGender":
                HandleGenderVoucher(session, item);
                break;
            case "SurvivalLevelExp":
                HandleSurvivalLevelExp(session, item);
                break;
            case "ItemSocketScroll":
                HandleItemSocketScroll(session, item);
                break;
            case "EnchantScroll":
                HandleEnchantScroll(session, item);
                break;
            default:
                Logger.Warning("Unhandled item function: {functionName}", item.Function.Name);
                break;
        }

        if (item.TransferType == TransferType.BindOnUse & !item.IsBound())
        {
            item.BindItem(session.Player);
        }
    }

    private static void HandleItemRemakeScroll(GameSession session, long itemUid)
    {
        session.Send(ChangeAttributesScrollPacket.Open(itemUid));
    }

    private static void HandleChatEmoticonAdd(GameSession session, Item item)
    {
        long expiration = long.MaxValue;

        if (item.Function.ChatEmoticonAdd.Duration > 0)
        {
            expiration = item.Function.ChatEmoticonAdd.Duration + TimeInfo.Now();
        }

        // sticker exists and no expiration
        if (session.Player.ChatSticker.Any(p => p.GroupId == item.Function.ChatEmoticonAdd.Id && p.Expiration == long.MaxValue))
        {
            return;
        }

        // Add time if the sticker is already in the list
        ChatSticker sticker = session.Player.ChatSticker.FirstOrDefault(p => p.GroupId == item.Function.ChatEmoticonAdd.Id);
        if (sticker is not null && sticker.Expiration != long.MaxValue)
        {
            expiration += sticker.Expiration - TimeInfo.Now();
        }

        session.Send(ChatStickerPacket.AddSticker(item.Id, item.Function.ChatEmoticonAdd.Id, expiration));
        session.Player.ChatSticker.Add(new((byte) item.Function.ChatEmoticonAdd.Id, expiration));
        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
    }

    private static void HandleSelectItemBox(GameSession session, PacketReader packet, Item item)
    {
        short boxType = packet.ReadShort();
        int index = packet.ReadShort() - 0x30;

        ItemBoxHelper.GiveItemFromSelectBox(session, item, index, out _);
    }

    private static void HandleOpenItemBox(GameSession session, PacketReader packet, Item item)
    {
        short boxType = packet.ReadShort();

        ItemBoxHelper.GiveItemFromOpenBox(session, item, out _);
    }

    private static void HandleOpenMassive(GameSession session, PacketReader packet, Item item)
    {
        // Major WIP

        string password = packet.ReadUnicodeString();
        int duration = item.Function.OpenMassiveEvent.Duration + Environment.TickCount;
        CoordF portalCoord = session.Player.FieldPlayer.Coord;
        CoordF portalRotation = session.Player.FieldPlayer.Rotation;

        session.FieldManager.BroadcastPacket(PlayerHostPacket.StartMinigame(session.Player, item.Function.OpenMassiveEvent.FieldId));
        //  session.FieldManager.BroadcastPacket(FieldPacket.AddPortal()
        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
    }

    private static void HandleLevelPotion(GameSession session, Item item)
    {
        if (session.Player.Levels.Level >= item.Function.LevelPotion.TargetLevel)
        {
            return;
        }

        session.Player.Levels.SetLevel(item.Function.LevelPotion.TargetLevel);

        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
    }

    private static void HandleTitleScroll(GameSession session, Item item)
    {
        if (session.Player.Titles.Contains(item.Function.Id))
        {
            return;
        }

        session.Player.Titles.Add(item.Function.Id);

        session.Send(UserEnvPacket.AddTitle(item.Function.Id));

        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
    }

    private static void HandleOpenInstrument(Item item)
    {
        if (!InstrumentCategoryInfoMetadataStorage.IsValid(item.Function.Id))
        {
            return;
        }
    }

    private static void HandleVIPCoupon(GameSession session, Item item)
    {
        long vipTime = item.Function.VIPCoupon.Duration * 3600;

        PremiumClubHandler.ActivatePremium(session, vipTime);
        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
    }

    private static void HandleStoryBook(GameSession session, Item item)
    {
        session.Send(StoryBookPacket.Open(item.Function.Id));
    }

    private static void HandleHongBao(GameSession session, Item item)
    {
        HongBao newHongBao = new(session.Player, item.Function.HongBao.TotalUsers, item.Id, item.Function.HongBao.Id, item.Function.HongBao.Count,
            item.Function.HongBao.Duration);
        GameServer.HongBaoManager.AddHongBao(newHongBao);

        session.FieldManager.BroadcastPacket(PlayerHostPacket.OpenHongbao(session.Player, newHongBao));
        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
    }

    private static void HandleOpenGachaBox(GameSession session, PacketReader packet, Item capsule)
    {
        string amount = packet.ReadUnicodeString();
        int rollCount = 0;

        GachaMetadata gacha = GachaMetadataStorage.GetMetadata(capsule.Function.Id);

        List<Item> items = new();

        if (amount == "single")
        {
            rollCount = 1;
        }
        else if (amount == "multi")
        {
            rollCount = 10;
        }

        for (int i = 0; i < rollCount; i++)
        {
            GachaContent contents = HandleSmartGender(gacha, session.Player.Gender);

            int itemAmount = Random.Shared.Next(contents.MinAmount, contents.MaxAmount);

            Item gachaItem = new(contents.ItemId, itemAmount, contents.Rarity)
            {
                GachaDismantleId = gacha.GachaId
            };
            items.Add(gachaItem);
            session.Player.Inventory.ConsumeItem(session, capsule.Uid, 1);
        }

        session.Send(FireWorksPacket.Gacha(items));

        foreach (Item item in items)
        {
            session.Player.Inventory.AddItem(session, item, true);
        }
    }

    private static GachaContent HandleSmartGender(GachaMetadata gacha, Gender playerGender)
    {
        Random random = Random.Shared;
        int index = random.Next(gacha.Contents.Count);

        GachaContent contents = gacha.Contents[index];
        if (!contents.SmartGender)
        {
            return contents;
        }

        Gender itemGender = ItemMetadataStorage.GetLimitMetadata(contents.ItemId).Gender;
        if (playerGender != itemGender || itemGender != Gender.Neutral) // if it's not the same gender or unisex, roll again
        {
            bool sameGender = false;
            do
            {
                int indexReroll = random.Next(gacha.Contents.Count);

                GachaContent rerollContents = gacha.Contents[indexReroll];
                Gender rerollContentsGender = ItemMetadataStorage.GetLimitMetadata(rerollContents.ItemId).Gender;

                if (rerollContentsGender == playerGender || rerollContentsGender == Gender.Neutral)
                {
                    return rerollContents;
                }
            } while (!sameGender);
        }

        return contents;
    }

    private static void HandleOpenCoupleEffectBox(GameSession session, PacketReader packet, Item item)
    {
        string targetUser = packet.ReadUnicodeString();

        if (targetUser == session.Player.Name)
        {
            session.Send(NoticePacket.Notice(SystemNotice.CoupleEffectErrorOpenboxMyselfChar, NoticeType.Popup));
            return;
        }

        if (!DatabaseManager.Characters.NameExists(targetUser))
        {
            session.Send(NoticePacket.Notice(SystemNotice.SpecifiedCharacterCouldNotBeFound, NoticeType.Popup));
            return;
        }

        Player otherPlayer = GameServer.PlayerManager.GetPlayerByName(targetUser);
        if (otherPlayer == null)
        {
            otherPlayer = DatabaseManager.Characters.FindPartialPlayerByName(targetUser);
        }

        if (otherPlayer.AccountId == session.Player.AccountId)
        {
            session.Send(NoticePacket.Notice(SystemNotice.CoupleEffectErrorOpenboxMyselfAccount, NoticeType.Popup));
            return;
        }

        Item badge = new(item.Function.OpenCoupleEffectBox.Id, rarity: item.Function.OpenCoupleEffectBox.Rarity)
        {
            PairedCharacterId = otherPlayer.CharacterId,
            PairedCharacterName = otherPlayer.Name
        };

        Item otherUserBadge = new(item.Function.OpenCoupleEffectBox.Id, rarity: item.Function.OpenCoupleEffectBox.Rarity)
        {
            PairedCharacterId = session.Player.CharacterId,
            PairedCharacterName = session.Player.Name
        };

        List<Item> items = new()
        {
            otherUserBadge
        };

        MailHelper.SendMail(MailType.System, otherPlayer.CharacterId, session.Player.CharacterId,
            "<ms2><v key=\"s_couple_effect_mail_sender\" /></ms2>",
            "<ms2><v key=\"s_couple_effect_mail_title_receiver\" /></ms2>",
            "<ms2><v key=\"s_couple_effect_mail_content_receiver\" /></ms2>",
            "",
            $"<ms2><v str=\"{session.Player.Name}\" ></v></ms2>",
            items,
            0, 0, out Mail mail);

        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
        session.Player.Inventory.AddItem(session, badge, true);
        List<string> noticeParameters = new()
        {
            otherPlayer.Name
        };

        session.Send(NoticePacket.Notice(SystemNotice.YouMailedBuddyBadgeToOtherPlayer, NoticeType.Chat | NoticeType.FastText, noticeParameters));
    }

    private static void HandlePetExtraction(GameSession session, PacketReader packet, Item item)
    {
        long petUid = long.Parse(packet.ReadUnicodeString());
        if (!session.Player.Inventory.HasItem(petUid))
        {
            return;
        }

        Item pet = session.Player.Inventory.GetByUid(petUid);

        Item badge = new(70100000)
        {
            PetSkinBadgeId = pet.Id,
            CreationTime = TimeInfo.Now() + Environment.TickCount
        };

        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
        session.Player.Inventory.AddItem(session, badge, true);
        session.Send(PetSkinPacket.Extract(petUid, badge));
    }

    private static void HandleCallAirTaxi(GameSession session, PacketReader packet, Item item)
    {
        int fieldID = int.Parse(packet.ReadUnicodeString());
        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
        session.Player.Warp(fieldID);
    }

    private static void HandleInstallBillBoard(GameSession session, PacketReader packet, Item item)
    {
        string[] parameters = packet.ReadUnicodeString().Split("'");
        string title = parameters[0];
        string description = parameters[1];
        bool publicHouse = parameters[2].Equals("1");

        int balloonUid = GuidGenerator.Int();
        string id = "AdBalloon_" + balloonUid;
        IFieldObject<InteractObject> fieldAdBallon = session.FieldManager.AddAdBalloon(new(id, item.Function.InstallBillboard.InteractId,
            InteractObjectState.Default, InteractObjectType.AdBalloon,
            session.Player, item.Function.InstallBillboard, title, description, publicHouse));
        session.FieldManager.State.AddInteractObject(fieldAdBallon);
        session.FieldManager.BroadcastPacket(InteractObjectPacket.Add(fieldAdBallon));
        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
    }

    private static void HandleExpandCharacterSlot(GameSession session, Item item)
    {
        Account account = DatabaseManager.Accounts.FindById(session.Player.AccountId);
        int maxSlots = int.Parse(ConstantsMetadataStorage.GetConstant("MaxCharacterSlots"));
        if (account.CharacterSlots >= maxSlots)
        {
            session.Send(CouponUsePacket.MaxCharacterSlots());
            return;
        }

        account.CharacterSlots++;
        DatabaseManager.Accounts.Update(account);
        session.Send(CouponUsePacket.CharacterSlotAdded());
        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
    }

    private static void HandleBeautyVoucher(GameSession session, Item item)
    {
        if (item.Gender != session.Player.Gender)
        {
            return;
        }

        session.Send(CouponUsePacket.BeautyCoupon(session.Player.FieldPlayer, item.Uid));
    }

    private static void HandleRepackingScroll(GameSession session, Item item)
    {
        session.Send(ItemRepackagePacket.Open(item.Uid));
    }

    private static void HandleNameVoucher(GameSession session, PacketReader packet, Item item)
    {
        string newName = packet.ReadUnicodeString();
        string oldName = session.Player.Name;
        session.Player.Name = newName;

        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);

        session.Send(CharacterListPacket.NameChanged(session.Player.CharacterId, newName));

        // Update name on socials
        foreach (Club club in session.Player.Clubs)
        {
            club.BroadcastPacketClub(ClubPacket.UpdateMemberName(oldName, newName, session.Player.CharacterId));
            if (club.LeaderCharacterId == session.Player.CharacterId)
            {
                club.LeaderName = newName;
            }
        }

        if (session.Player.Guild is not null)
        {
            session.Player.Guild.BroadcastPacketGuild(GuildPacket.UpdateMemberName(oldName, newName));
            if (session.Player.Guild.LeaderCharacterId == session.Player.CharacterId)
            {
                session.Player.Guild.LeaderName = newName;
            }
        }

        session.Player.Party?.BroadcastPacketParty(PartyPacket.UpdatePlayer(session.Player));

        // TODO: Needs to redirect player to character selection screen after pop-up
    }

    private static void HandleGenderVoucher(GameSession session, Item item)
    {
        session.Player.Gender = session.Player.Gender == Gender.Male ? Gender.Female : Gender.Male;

        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
        ChangeToDefaultHair(session);
        ChangeToDefaultFace(session);
        session.Send(NoticePacket.QuitNotice(SystemNotice.ChangeGenderResultSuccess, NoticeType.Popup));
    }

    private static void ChangeToDefaultHair(GameSession session)
    {
        int hairId = session.Player.Gender == Gender.Male
            ? 10200003 // Mega Mop-Top
            : 10200012; // Cutesy Twin Tails

        BeautyHelper.ChangeHair(session, hairId, out _, out _);
    }

    private static void ChangeToDefaultFace(GameSession session)
    {
        int faceId = session.Player.Gender == Gender.Male
            ? 10300002 // Mega Mop-Top
            : 10300004; // Bookworm

        BeautyHelper.ChangeFace(session, faceId, out _, out _);
    }

    private static void HandleSurvivalLevelExp(GameSession session, Item item)
    {
        session.Player.Account.MushkingRoyaleStats.AddExp(session, item.Function.SurvivalLevelExp.SurvivalExp);
    }

    private static void HandleItemSocketScroll(GameSession session, Item item)
    {
        ItemSocketScrollMetadata? metadata = ItemSocketScrollMetadataStorage.GetMetadata(item.Function.Id);
        if (metadata is null)
        {
            return;
        }

        byte socketCount = ItemSocketScrollHelper.GetSocketCount(metadata.Id);
        int successRate = (int) ItemSocketScrollHelper.GetSuccessRate(metadata.Id) * 10000;
        session.Send(ItemSocketScrollPacket.OpenWindow(item.Uid, successRate, socketCount));
    }

    private static void HandleEnchantScroll(GameSession session, Item item)
    {
        EnchantScrollMetadata metadata = EnchantScrollMetadataStorage.GetMetadata(item.Function.Id);
        if (metadata is null)
        {
            return;
        }

        Script script = ScriptLoader.GetScript("Functions/ItemEnchantScroll/getSuccessRate");
        float successRate = (float) script.RunFunction("getSuccessRate", metadata.Id).Number;
        session.Send(EnchantScrollPacket.OpenWindow(item.Uid, metadata, successRate));
    }
}
