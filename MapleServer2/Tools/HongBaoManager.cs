using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class HongBaoManager
    {
        private readonly Dictionary<long, HongBao> HongBaoList;

        public HongBaoManager()
        {
            HongBaoList = new Dictionary<long, HongBao>();
        }

        public void AddHongBao(HongBao hongBao)
        {
            HongBaoList.Add(hongBao.Id, hongBao);
        }

        public void RemoveHongBao(HongBao hongBao)
        {
            HongBaoList.Remove(hongBao.Id);
        }

        public HongBao GetHongBaoById(int id)
        {
            return HongBaoList.TryGetValue(id, out HongBao foundHongBao) ? foundHongBao : null;
        }
    }
}
