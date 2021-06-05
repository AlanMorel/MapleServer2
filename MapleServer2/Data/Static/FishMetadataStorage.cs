﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class FishMetadataStorage
    {
        private static readonly Dictionary<int, FishMetadata> map = new Dictionary<int, FishMetadata>();

        static FishMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-fish-metadata");
            List<FishMetadata> items = Serializer.Deserialize<List<FishMetadata>>(stream);
            foreach (FishMetadata item in items)
            {
                map[item.Id] = item;
            }
        }

        public static bool IsValid(int fishId)
        {
            return map.ContainsKey(fishId);
        }

        public static FishMetadata GetMetadata(int fishId)
        {
            return map.GetValueOrDefault(fishId);
        }

        public static List<FishMetadata> GetValidFishes(int mapId, string habitat)
        {
            if (habitat == "water") // temp fix for maps with seawater. 
            {
                return map.Values.Where(x => x.HabitatMapId.Contains(mapId) &&
                (x.Habitat == habitat || x.Habitat == "seawater")).ToList();
            }
            return map.Values.Where(x => x.HabitatMapId.Contains(mapId) && x.Habitat == habitat).ToList();
        }
    }
}
