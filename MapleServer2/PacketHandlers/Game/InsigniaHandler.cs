using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class InsigniaHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.NAMETAG_SYMBOL;

        public InsigniaHandler(ILogger<InsigniaHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {

            ProcessInsignias(session);

            short insigniaId = packet.ReadShort();

            if (insigniaId < 0)
            {
                return;
            }

            if (session.Player.AvaliableInsignias.Contains(insigniaId))
            {
                session.Player.InsigniaId = insigniaId;
            }
            else
            {
                insigniaId = 0;
            }

            session.FieldManager.BroadcastPacket(InsigniaPacket.UpdateInsignia(session, insigniaId, insigniaId != 0));
        }

        private void ProcessInsignias(GameSession session)
        {
            Player player = session.Player;
            int[] trophy = player.Trophy;
            List<int> avaliableTitles = player.AvaliableTitles;
            List<short> avaliableInsignias = player.AvaliableInsignias;
            Dictionary<ItemSlot, Item> equips = player.Equips;

            Dictionary<int, short> titlesThatGiveInsignias = new Dictionary<int, short> {
                {10000569, 2}, {10000152, 3}, {10000570, 4}, {10000196, 6}, {10000195, 7}, {10000571, 14}, {10000331, 15}, {10000190, 22},
                {10000458, 28}, {10000465, 30 }, {10000503, 33}, {10000512, 35}, {10000513, 36}, {10000514, 37}, {10000537, 38}, {10000565, 41},
                {10000602, 43}, {10000603, 44}, {10000638, 45 }, {10000644, 46}
            };

            // TODO: Missing checks for Mushking Royale lv 5 and Premium Club member
            int trophyCount = trophy[0] + trophy[1] + trophy[2];

            if (avaliableTitles.Contains(10000171) && trophyCount >= 1000 && !avaliableInsignias.Contains(5))
            {
                avaliableInsignias.Add(5);
            }

            if (player.Level >= 50 && !avaliableInsignias.Contains(11))
            {
                avaliableInsignias.Add(11);
            }

            if (player.PrestigeLevel == 100 && !avaliableInsignias.Contains(29))
            {
                avaliableInsignias.Add(29);
            }

            foreach (KeyValuePair<int, short> item in titlesThatGiveInsignias)
            {
                if (!avaliableInsignias.Contains(item.Value) && avaliableTitles.Contains(item.Key))
                {
                    avaliableInsignias.Add(item.Value);
                }
            }

            if (!avaliableInsignias.Contains(1))
            {
                foreach (KeyValuePair<ItemSlot, Item> item in equips)
                {
                    if (item.Value.Enchants >= 12 && item.Value.Rarity >= 4 && !avaliableInsignias.Contains(1))
                    {
                        avaliableInsignias.Add(1);
                    }
                }
            }
        }
    }
}
