using System;
using System.Collections;
using System.Collections.Generic;
using Maple2Storage.Types;
using Maple2Storage.Utils;
using MaplePacketLib2.Tools;

namespace Maple2.Data.Types.Items {
    public class ItemSockets : IByteSerializable, IEnumerable<Item> {
        public byte MaxSockets { get; private set; }

        public Item this[int i] => sockets[i];

        public byte Unlocked {
            get => (byte) sockets.Length;
            private set => Array.Resize(ref sockets, value);
        }

        private Item[] sockets;

        public ItemSockets(byte maxSockets, byte unlocked) {
            this.MaxSockets = maxSockets;
            this.sockets = new Item[unlocked];
        }

        public void Load(IEnumerable<Item> gems) {
            foreach (Item gem in gems) {
                EquipGemstone((byte) gem.Slot, gem);
            }
        }

        public ItemSockets(ItemSockets other) {
            MaxSockets = other.MaxSockets;
            sockets = new Item[other.sockets.Length];
            for (int i = 0; i < sockets.Length; i++) {
                if (other.sockets[i] != default) {
                    sockets[i] = other.sockets[i].Clone();
                }
            }
        }

        public bool CanUnlockSocket() {
            return sockets.Length < MaxSockets;
        }

        public bool UnlockSocket() {
            if (sockets.Length >= MaxSockets) {
                return false;
            }

            Unlocked = (byte) (sockets.Length + 1);
            return true;
        }

        public bool EquipGemstone(byte socketSlot, Item gem) {
            if (!SocketEmpty(socketSlot)) {
                return false;
            }

            gem.Slot = socketSlot;
            sockets[socketSlot] = gem;
            return true;
        }

        public bool UnequipGemstone(byte socketSlot, out Item gem) {
            if (!SocketTaken(socketSlot)) {
                gem = default;
                return false;
            }

            gem = sockets[socketSlot];
            gem.Slot = -1;
            sockets[socketSlot] = default;
            return true;
        }

        public bool SocketTaken(short socketSlot) {
            return socketSlot >= 0 && socketSlot < Unlocked && sockets[socketSlot] != default;
        }

        public bool SocketEmpty(short socketSlot) {
            return socketSlot >= 0 && socketSlot < Unlocked && sockets[socketSlot] == default;
        }

        public void WriteTo(IByteWriter writer) {
            writer.WriteByte(MaxSockets);
            writer.WriteByte(Unlocked);
            for (int i = 0; i < Unlocked; i++) {
                writer.WriteBool(sockets[i] != default);
                if (sockets[i] != default) {
                    Item gem = sockets[i];
                    writer.WriteInt(gem.MapleId);
                    /*bool isBound = gem.Transfer.Binding != null;
                    writer.WriteBool(isBound);
                    if (isBound) {
                        writer.WriteClass<ItemBinding>(gem.Transfer.Binding);
                    }*/

                    writer.WriteBool(gem.IsLocked);
                    if (gem.IsLocked) {
                        writer.WriteByte();
                        writer.WriteLong(gem.UnlockTime);
                    }
                }
            }
        }

        public void ReadFrom(IByteReader reader) {
            throw new NotImplementedException();
        }

        public IEnumerator<Item> GetEnumerator() {
            for (int i = 0; i < Unlocked; i++) {
                if (sockets[i] == null) continue;
                yield return sockets[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}