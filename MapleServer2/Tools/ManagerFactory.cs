using System;
using System.Collections.Generic;
using MapleServer2.Servers.Game;

namespace MapleServer2.Tools
{
    public class ManagerFactory<T> where T : class
    {
        private readonly Dictionary<int, CacheItem> Managers;

        public ManagerFactory()
        {
            Managers = new Dictionary<int, CacheItem>();
        }

        public T GetManager(int key)
        {
            lock (Managers)
            {
                if (!Managers.TryGetValue(key, out CacheItem item))
                {
                    item = new CacheItem(CreateInstance(key));
                    Managers[key] = item;
                }

                item.Pin();
                return item.Value;
            }
        }

        public bool Release(int key)
        {
            lock (Managers)
            {
                if (!Managers.TryGetValue(key, out CacheItem manager) || manager.Release() > 0)
                {
                    return false;
                }

                return Managers.Remove(key);
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
            private int Count;

            public CacheItem(T value)
            {
                Value = value;
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
