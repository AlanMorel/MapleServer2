using System;
using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class PlayerStorage
    {
        private readonly Dictionary<long, Player> idStorage;
        private readonly Dictionary<string, Player> nameStorage;

        public PlayerStorage()
        {
            this.idStorage = new Dictionary<long, Player>();
            this.nameStorage = new Dictionary<string, Player>();
        }

        public void AddPlayer(Player player)
        {
            idStorage.Add(player.CharacterId, player);
            nameStorage.Add(player.Name, player);
        }

        public void RemovePlayer(Player player)
        {
            idStorage.Remove(player.CharacterId);
            nameStorage.Remove(player.Name);
        }

        public Player GetPlayerByName(string name)
        {
            return nameStorage[name];
        }

        public Player GetPlayerById(long id)
        {
            return idStorage[id];
        }
    }
}
