﻿using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class GachaMetadataStorage
    {
        private static readonly Dictionary<int, GachaMetadata> gacha = new Dictionary<int, GachaMetadata>();

        static GachaMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-gacha-metadata");
            List<GachaMetadata> items = Serializer.Deserialize<List<GachaMetadata>>(stream);
            foreach (GachaMetadata item in items)
            {
                gacha[item.GachaId] = item;
            }
        }

        public static bool IsValid(int gachaId)
        {
            return gacha.ContainsKey(gachaId);
        }

        public static GachaMetadata GetMetadata(int gachaId)
        {
            return gacha.GetValueOrDefault(gachaId);
        }
    }
}
