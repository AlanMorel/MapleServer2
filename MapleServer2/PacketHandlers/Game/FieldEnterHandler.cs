using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class FieldEnterHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.RESPONSE_FIELD_ENTER;

    public FieldEnterHandler() : base() { }

    public override void Handle(GameSession session, PacketReader packet)
    {
        packet.ReadInt(); // ?

        // Liftable: 00 00 00 00 00
        // SendBreakable
        // Self
        Player player = session.Player;
        Account account = player.Account;
        session.EnterField(player);
        session.Send(StatPacket.SetStats(session.Player.FieldPlayer));
        session.Send(StatPointPacket.WriteTotalStatPoints(player));

        if (account.IsVip())
        {
            session.Send(BuffPacket.SendBuff(0, new(100000014, session.Player.FieldPlayer.ObjectId, session.Player.FieldPlayer.ObjectId, 1, (int) account.VIPExpiration, 1)));
            session.Send(PremiumClubPacket.ActivatePremium(session.Player.FieldPlayer, account.VIPExpiration));
        }

        session.Send(EmotePacket.LoadEmotes(player));
        session.Send(ChatStickerPacket.LoadChatSticker(player));

        session.Send(HomeCommandPacket.LoadHome(player));
        session.Send(ResponseCubePacket.DecorationScore(account.Home));
        session.Send(ResponseCubePacket.LoadHome(session.Player.FieldPlayer.ObjectId, session.Player.Account.Home));
        session.Send(ResponseCubePacket.ReturnMap(player.ReturnMapId));
        session.Send(LapenshardPacket.Load(player.Inventory.LapenshardStorage));

        IEnumerable<Cube> cubes = session.FieldManager.State.Cubes.Values.Where(x => x.Value.PlotNumber == 1
                                                                                    && x.Value.Item.HousingCategory is ItemHousingCategory.Farming or ItemHousingCategory.Ranching).Select(x => x.Value);
        foreach (Cube cube in cubes)
        {
            session.Send(FunctionCubePacket.UpdateFunctionCube(cube.CoordF.ToByte(), 2, 1));
        }
        if (player.Party != null)
        {
            session.Send(PartyPacket.UpdatePlayer(player));
        }

        session.Send(KeyTablePacket.SendHotbars(player.GameOptions));

        List<GameEvent> gameEvents = DatabaseManager.Events.FindAll();
        session.Send(GameEventPacket.Load(gameEvents));
    }
}
