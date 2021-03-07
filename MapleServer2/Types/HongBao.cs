using System.Collections.Generic;
using System.Threading.Tasks;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class HongBao
    {
        public int Id { get; set; }
        public long Expiration { get; set; }
        public Player Giver { get; set; }
        public List<Player> Receivers { get; set; }
        public byte MaxReceivers { get; set; }
        public int ItemId { get; set; }
        public int RewardId { get; set; }
        public short RewardAmount { get; set; }
        public int Duration { get; set; }
        public bool Active { get; set; }

        public HongBao() { }

        public HongBao(Player giver, byte totalUserCount, int itemId, int rewardId, short rewardAmount, int duration)
        {
            Id = GuidGenerator.Int();
            Expiration = 0;
            Receivers = new List<Player> { };
            Giver = giver;
            MaxReceivers = (byte) (totalUserCount - 1); // subtract one because the giver already received one
            ItemId = itemId;
            RewardId = rewardId;
            RewardAmount = rewardAmount;
            Duration = duration;
            Active = true;

            giver.Session.Send(MeretsPacket.UpdateMerets(giver.Session, rewardAmount));
            giver.Wallet.EventMeret.Modify(rewardAmount);
            Start(this);
        }

        public void AddReceiver(Player player)
        {
            if (Receivers.Contains(player) || player == Giver)
            {
                return;
            }

            Receivers.Add(player);

            if (MaxReceivers == Receivers.Count)
            {
                DistributeReward();
            }
        }

        public void DistributeReward()
        {
            if (Active == false)
            {
                return;
            }

            if (Receivers.Count == 0) // Nobody joined
            {
                Active = false;
                GameServer.HongBaoManager.RemoveHongBao(this);
                // TODO find any system or chat notice packet
                return;
            }
            short dividedAwardAmount = (short) (RewardAmount / Receivers.Count);
            foreach (Player player in Receivers)
            {
                player.Session.FieldManager.BroadcastPacket(PlayerHostPacket.HongbaoGiftNotice(player, this, dividedAwardAmount));
                player.Session.Send(MeretsPacket.UpdateMerets(player.Session, dividedAwardAmount));
                player.Wallet.EventMeret.Modify(dividedAwardAmount);
            }

            Active = false;
            GameServer.HongBaoManager.RemoveHongBao(this);
        }

        private static async Task Start(HongBao hongBao)
        {
            await Task.Delay(hongBao.Duration * 1000);

            hongBao.DistributeReward();
        }
    }
}
