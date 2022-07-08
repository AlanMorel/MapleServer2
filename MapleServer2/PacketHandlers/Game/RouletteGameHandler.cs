using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

public class RouletteGameHandler : GamePacketHandler<RouletteGameHandler>
{
    public override RecvOp OpCode => RecvOp.RouletteGame;

    private enum Mode : byte
    {
        Open = 0x00,
        Spin = 0x02,
        Close = 0x03
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();
        switch (mode)
        {
            case Mode.Open:
                HandleOpen(session, packet);
                break;
            case Mode.Spin:
                HandleSpin(session);
                break;
            case Mode.Close:
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpen(GameSession session, PacketReader packet)
    {
        int rouletteId = packet.ReadInt();

        List<RouletteGameItem> items = DatabaseManager.RouletteGameItems.FindAllByRouletteId(rouletteId);
        if (items.Count == 0)
        {
            return;
        }

        session.Player.RouletteId = rouletteId;
        session.Send(RouletteGamePacket.OpenWheel(items));
    }

    private static void HandleSpin(GameSession session)
    {
        List<RouletteGameItem> items = DatabaseManager.RouletteGameItems.FindAllByRouletteId(session.Player.RouletteId);
        if (items.Count == 0)
        {
            return;
        }

        Script luaScript = ScriptLoader.GetScript($"Npcs/{session.Player.NpcTalk.Npc.Id}", session);
        DynValue result = luaScript?.RunFunction("rouletteSpin", session.Player.NpcTalk.ScriptId);
        if (result == null)
        {
            return;
        }

        int tokenItemId = (int) result.Tuple[0].Number;
        int spinCount = (int) result.Tuple[1].Number;
        int tokenSpinCost = (int) result.Tuple[2].Number;

        int totalTokenSpinCost = spinCount * tokenSpinCost;
        Item token = session.Player.Inventory.GetById(tokenItemId);
        if (token.Amount < totalTokenSpinCost)
        {
            return;
        }

        session.Player.Inventory.ConsumeItem(session, token.Uid, totalTokenSpinCost);

        List<int> randomIndexes = new();
        for (int i = 0; i < spinCount; i++)
        {
            int randomIndex = Random.Shared.Next(0, items.Count);
            randomIndexes.Add(randomIndex);
            Item item = new(items[randomIndex].ItemId, items[randomIndex].ItemAmount, items[randomIndex].ItemRarity);
            session.Player.Inventory.AddItem(session, item, true);
        }

        session.Send(RouletteGamePacket.SpinWheel(randomIndexes, items));
    }
}
