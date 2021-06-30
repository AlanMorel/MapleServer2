using System;
using System.Collections.Generic;
using System.Linq;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class ManagerFactory<T> where T : class
    {
        private readonly Dictionary<int, List<CacheItem>> Managers;

        public ManagerFactory()
        {
            Managers = new Dictionary<int, List<CacheItem>>();
        }

        public T GetManager(int key, int instanceId)
        {
            lock (Managers)
            {
                if (!Managers.TryGetValue(key, out List<CacheItem> list))
                {
                    list = new List<CacheItem>() { new CacheItem(CreateInstance(key), instanceId) };
                    Managers[key] = list;
                }

                CacheItem manager = list.FirstOrDefault(x => x.InstanceId == instanceId);
                if (manager == default)
                {
                    manager = new CacheItem(CreateInstance(key), instanceId);
                    list.Add(manager);
                }
                manager.Pin();
                return manager.Value;
            }
        }

        public bool Release(int key, int instanceId, Player player)
        {
            lock (Managers)
            {
                if (!Managers.TryGetValue(key, out List<CacheItem> manager))
                {
                    return false;
                }

                CacheItem item = manager.FirstOrDefault(x => x.InstanceId == instanceId);
                if (item == default || item.Release() > 0)
                {
                    return false;
                }

                if (player.DungeonSessionId != -1)
                {
                    player.DungeonSessionId = -1;
                    GameServer.DungeonManager.RemoveDungeonSession(player.DungeonSessionId);
                }

                
                    //controlled field manager
               // if dungeonmap

                return manager.Remove(item);
            }
        }

        // This is really hacky but...
        private static T CreateInstance(int key)
        {
            if (typeof(T) == typeof(FieldManager))
            {
                return new FieldManager(key) as T;
            }
            else
            {
                throw new ArgumentException("Unsupported Manager for ManagerFactory");
            }
        }

        private class CacheItem
        {
            public readonly T Value;
            public readonly int InstanceId;
            private int Count;

            public CacheItem(T value, int instanceId)
            {
                Value = value;
                InstanceId = instanceId;
                Count = 0;
            }

            public int Pin()
            {
                return ++Count;
            }

            public int Release()
            {
                return --Count;
            }
        }
    }
}
