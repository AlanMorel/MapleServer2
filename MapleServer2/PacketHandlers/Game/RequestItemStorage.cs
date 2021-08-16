using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Classes;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemStorage : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_STORAGE;

        public RequestItemStorage(ILogger<RequestItemStorage> logger) : base(logger) { }

        private enum ItemStorageMode : byte
        {
            Add = 0x00,
            Remove = 0x01,
            Move = 0x02,
            Mesos = 0x03,
            Expand = 0x06,
            Sort = 0x08,
            LoadBank = 0x0C,
            Close = 0x0F,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ItemStorageMode mode = (ItemStorageMode) packet.ReadByte();

            switch (mode)
            {
                case ItemStorageMode.Add:
                    HandleAdd(session, packet);
                    break;
                case ItemStorageMode.Remove:
                    HandleRemove(session, packet);
                    break;
                case ItemStorageMode.Move:
                    HandleMove(session, packet);
                    break;
                case ItemStorageMode.Mesos:
                    HandleMesos(session, packet);
                    break;
                case ItemStorageMode.Expand:
                    HandleExpand(session);
                    break;
                case ItemStorageMode.Sort:
                    HandleSort(session);
                    break;
                case ItemStorageMode.LoadBank:
                    HandleLoadBank(session);
                    break;
                case ItemStorageMode.Close:
                    HandleClose(session.Player);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleAdd(GameSession session, PacketReader packet)
        {
            packet.ReadLong();
            long uid = packet.ReadLong();
            short slot = packet.ReadShort();
            int amount = packet.ReadInt();

            session.Player.BankInventory.Add(session, uid, amount, slot);
        }

        private static void HandleRemove(GameSession session, PacketReader packet)
        {
            packet.ReadLong();
            long uid = packet.ReadLong();
            short slot = packet.ReadShort();
            int amount = packet.ReadInt();
            if (!session.Player.BankInventory.Remove(session, uid, slot, amount, out Item item))
            {
                return;
            }
            item.Slot = slot;
            InventoryController.Add(session, item, false);
        }

        private static void HandleMove(GameSession session, PacketReader packet)
        {
            packet.ReadLong();
            long uid = packet.ReadLong();
            short slot = packet.ReadShort();

            session.Player.BankInventory.Move(session, uid, slot);
        }

        private static void HandleMesos(GameSession session, PacketReader packet)
        {
            packet.ReadLong();
            byte mode = packet.ReadByte();
            long amount = packet.ReadLong();
            Wallet wallet = session.Player.Wallet;

            if (mode == 1) // add mesos
            {
                if (wallet.Meso.Modify(-amount))
                {
                    wallet.Bank.Modify(amount);
                }
            }
            else if (mode == 0) // remove mesos
            {
                if (wallet.Bank.Modify(-amount))
                {
                    wallet.Meso.Modify(amount);
                }
            }
        }

        private static void HandleExpand(GameSession session)
        {
            session.Player.BankInventory.Expand(session);
        }

        private static void HandleSort(GameSession session)
        {
            session.Send(StorageInventoryPacket.Update());
            session.Player.BankInventory.Sort(session);
        }

        private static void HandleLoadBank(GameSession session)
        {
            session.Player.BankInventory.LoadBank(session);
        }

        private static void HandleClose(Player player)
        {
            DatabaseCharacter.Update(player);
        }
    }
}
