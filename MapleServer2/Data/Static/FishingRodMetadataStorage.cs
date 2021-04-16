﻿using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class FishingRodMetadataStorage
    {
        private static readonly Dictionary<int, FishingRodMetadata> map = new Dictionary<int, FishingRodMetadata>();

        static FishingRodMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-fishing-rod-metadata");
            List<FishingRodMetadata> items = Serializer.Deserialize<List<FishingRodMetadata>>(stream);
            foreach (FishingRodMetadata item in items)
            {
                map[item.RodId] = item;
            }
        }

        public static bool IsValid(int rodId)
        {
            return map.ContainsKey(rodId);
        }

        public static FishingRodMetadata GetMetadata(int rodId)
        {
            return map.GetValueOrDefault(rodId);
        }
    }
}
