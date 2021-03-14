using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public interface ItemStat { }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 10)]
    public struct NormalStat : ItemStat
    {
        public ItemAttribute Id { get; private set; }
        public int Flat { get; private set; }
        public float Percent { get; private set; }

        public static NormalStat Of(ItemAttribute type, int flat)
        {
            return new NormalStat
            {
                Id = type,
                Flat = flat,
                Percent = 0,
            };
        }

        public static NormalStat Of(ItemAttribute type, float percent)
        {
            return new NormalStat
            {
                Id = type,
                Flat = 0,
                Percent = percent,
            };
        }

        public static NormalStat Of(ParserStat stat)
        {
            return new NormalStat
            {
                Id = stat.Id,
                Flat = stat.Flat,
                Percent = stat.Percent,
            };
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 10)]
    public struct SpecialStat : ItemStat
    {
        public SpecialItemAttribute Id { get; private set; }
        public float Percent { get; private set; }
        public float Flat { get; private set; }

        public static SpecialStat Of(SpecialItemAttribute type, float percent, float flat)
        {
            return new SpecialStat
            {
                Id = type,
                Percent = percent,
                Flat = flat,
            };
        }

        public static SpecialStat Of(ParserSpecialStat stat)
        {
            return new SpecialStat
            {
                Id = stat.Id,
                Percent = stat.Percent,
                Flat = stat.Flat,
            };
        }
    }

    public class Gemstone
    {
        public readonly int Id;

        // Used if bound
        public readonly long OwnerId = 0;
        public readonly string OwnerName = "";

        public readonly long Unknown = 0;

        public Gemstone(int id)
        {
            Id = id;
        }
    }

    public class ItemStats
    {
        public readonly List<NormalStat> BasicAttributes;
        public readonly List<SpecialStat> SpecialBasicAttributes;
        public readonly List<NormalStat> BonusAttributes;
        public readonly List<SpecialStat> SpecialBonusAttributes;

        public byte TotalSockets;
        public readonly List<Gemstone> Gemstones;

        public ItemStats(int itemId, int rarity)
        {
            BasicAttributes = new List<NormalStat>();
            SpecialBasicAttributes = new List<SpecialStat>();
            BonusAttributes = new List<NormalStat>();
            SpecialBonusAttributes = new List<SpecialStat>();
            Gemstones = new List<Gemstone>();
            if (rarity == 0)
            {
                return;
            }

            if (ItemOptionsMetadataStorage.GetBasic(itemId, out List<ItemOption> basicList))
            {
                ItemOption itemOptions = basicList.Find(options => options.Rarity == rarity);
                if (itemOptions != null)
                {
                    // Weapon atk comes from each Item option and not from stat ranges
                    if (itemOptions.MaxWeaponAtk != 0)
                    {
                        BasicAttributes.Add(NormalStat.Of(ItemAttribute.MinWeaponAtk, itemOptions.MinWeaponAtk));
                        BasicAttributes.Add(NormalStat.Of(ItemAttribute.MaxWeaponAtk, itemOptions.MaxWeaponAtk));
                    }

                    List<ItemStat> itemStats = new List<ItemStat>();
                    foreach (ItemAttribute attribute in itemOptions.Stats)
                    {
                        itemStats.Add(NormalStat.Of(GetRange(itemId)[attribute][Roll()]));
                    }

                    foreach (SpecialItemAttribute attribute in itemOptions.SpecialStats)
                    {
                        itemStats.Add(SpecialStat.Of(GetSpecialRange(itemId)[attribute][Roll()]));
                    }

                    foreach (ItemStat stat in itemStats)
                    {
                        if (stat.GetType() == typeof(NormalStat))
                        {
                            BasicAttributes.Add((NormalStat) stat);
                        }
                        else
                        {
                            SpecialBasicAttributes.Add((SpecialStat) stat);
                        }
                    }
                }
            }

            if (ItemOptionsMetadataStorage.GetRandomBonus(itemId, out List<ItemOption> randomBonusList))
            {
                Random random = new Random();
                ItemOption itemoption = randomBonusList.FirstOrDefault(options => options.Rarity == rarity && options.Slots > 0);
                if (itemoption != null)
                {
                    List<ItemStat> itemStats = new List<ItemStat>();

                    foreach (ItemAttribute attribute in itemoption.Stats)
                    {
                        itemStats.Add(NormalStat.Of(GetRange(itemId)[attribute][Roll()]));
                    }

                    foreach (SpecialItemAttribute attribute in itemoption.SpecialStats)
                    {
                        itemStats.Add(SpecialStat.Of(GetSpecialRange(itemId)[attribute][Roll()]));
                    }

                    List<ItemStat> randomList = itemStats.OrderBy(x => random.Next()).Take(itemoption.Slots).ToList();

                    foreach (ItemStat stat in randomList)
                    {
                        if (stat.GetType() == typeof(NormalStat))
                        {
                            BonusAttributes.Add((NormalStat) stat);
                        }
                        else
                        {
                            SpecialBonusAttributes.Add((SpecialStat) stat);
                        }
                    }
                }
            }
        }

        public ItemStats(ItemStats other)
        {
            BasicAttributes = new List<NormalStat>(other.BasicAttributes);
            SpecialBasicAttributes = new List<SpecialStat>(other.SpecialBasicAttributes);
            BonusAttributes = new List<NormalStat>(other.BonusAttributes);
            SpecialBonusAttributes = new List<SpecialStat>(other.SpecialBonusAttributes);
            TotalSockets = other.TotalSockets;
            Gemstones = new List<Gemstone>(other.Gemstones);
        }

        private static Dictionary<ItemAttribute, List<ParserStat>> GetRange(int itemId)
        {
            ItemSlot slot = ItemMetadataStorage.GetSlot(itemId);
            if (Item.IsAccessory(slot))
            {
                return ItemOptionRangeStorage.GetAccessoryRanges();
            }

            if (Item.IsArmor(slot))
            {
                return ItemOptionRangeStorage.GetArmorRanges();
            }

            if (Item.IsWeapon(slot))
            {
                return ItemOptionRangeStorage.GetWeaponRanges();
            }

            return ItemOptionRangeStorage.GetPetRanges();
        }

        private static Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> GetSpecialRange(int itemId)
        {
            ItemSlot slot = ItemMetadataStorage.GetSlot(itemId);
            if (Item.IsAccessory(slot))
            {
                return ItemOptionRangeStorage.GetAccessorySpecialRanges();
            }

            if (Item.IsArmor(slot))
            {
                return ItemOptionRangeStorage.GetArmorSpecialRanges();
            }

            if (Item.IsWeapon(slot))
            {
                return ItemOptionRangeStorage.GetWeaponSpecialRanges();
            }

            return ItemOptionRangeStorage.GetPetSpecialRanges();
        }

        private static int Roll() // Returns index 0~7
        {
            Random random = new Random();
            return random.NextDouble() switch
            {
                >= 0.0 and < 0.24 => 0,
                >= 0.24 and < 0.48 => 1,
                >= 0.48 and < 0.74 => 2,
                >= 0.74 and < 0.9 => 3,
                >= 0.9 and < 0.966 => 4,
                >= 0.966 and < 0.985 => 5,
                >= 0.985 and < 0.9975 => 6,
                _ => 7,
            };
        }
    }
}
