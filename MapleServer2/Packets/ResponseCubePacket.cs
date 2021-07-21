using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ResponseCubePacket
    {
        private enum ResponseCubePacketMode : byte
        {
            LoadFurnishingItem = 0x1,
            EnablePlotFurnishing = 0x2,
            LoadPurchasedLand = 0x3,
            CompletePurchase = 0x4,
            ForfeitPlot = 0x5,
            ForfeitPlot2 = 0x7,
            PlaceFurnishing = 0xA,
            RemoveCube = 0xC,
            RotateCube = 0xE,
            ReplaceCube = 0xF,
            Pickup = 0x11,
            Drop = 0x12,
            LoadHome = 0x14,
            HomeName = 0x15,
            PurchasePlot = 0x16,
            ChangePassword = 0x18,
            ArchitectScoreExpiration = 0x19,
            KickEveryone = 0x1A,
            UpdateArchitectScore = 0x1C,
            HomeDescription = 0x1D,
            ReturnMap = 0x22,
            IncreaseSize = 0x25,
            DecreaseSize = 0x26,
            Rewards = 0x27,
            EnablePermission = 0x2A,
            SetPermission = 0x2B,
            IncreaseHeight = 0x2C,
            DecreaseHeight = 0x2D,
            ChangeBackground = 0x33,
            ChangeLighting = 0x34,
            GiveBuildingPermission = 0x35,
            ChangeCamera = 0x36,
            LoadWarehouseItems = 0x37,
            AddBuildingPermission = 0x39,
            RemoveBuildingPermission = 0x3A,
        }

        public static Packet LoadFurnishingItem(IFieldObject<Player> player, int itemId, long itemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.LoadFurnishingItem);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(itemId);
            pWriter.WriteLong(itemUid);
            pWriter.WriteLong();
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet EnablePlotFurnishing(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.EnablePlotFurnishing);
            pWriter.WriteByte(); // disable bool
            pWriter.WriteInt(player.Account.Home.PlotNumber);
            pWriter.WriteInt(player.Account.Home.ApartmentNumber);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteLong(player.Account.Home.Expiration);
            pWriter.WriteLong(player.CharacterId);

            return pWriter;
        }

        public static Packet CompletePurchase()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.CompletePurchase);

            return pWriter;
        }

        public static Packet RemovePlot(int plotNumber, int apartmentNumber)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ForfeitPlot);
            pWriter.WriteByte();
            pWriter.WriteInt(plotNumber);
            pWriter.WriteInt(apartmentNumber);
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet RemovePlot2(int plotId, int plotNumber)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ForfeitPlot2);
            pWriter.WriteByte(72); // unkwon
            pWriter.WriteShort();
            pWriter.WriteInt(plotId);
            pWriter.WriteInt(plotNumber);

            return pWriter;
        }

        public static Packet PlaceFurnishing(IFieldObject<Cube> cube, int ownerObjectId, int fieldPlayerObjectId, bool sendOnlyObjectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.PlaceFurnishing);
            pWriter.WriteBool(sendOnlyObjectId);
            pWriter.WriteInt(ownerObjectId);
            pWriter.WriteInt(fieldPlayerObjectId);

            if (sendOnlyObjectId)
            {
                return pWriter;
            }
            pWriter.WriteInt(cube.Value.PlotNumber);
            pWriter.WriteInt();
            pWriter.Write(cube.Coord.ToByte());
            pWriter.WriteByte();
            pWriter.WriteLong(cube.Value.Uid);
            pWriter.WriteInt(cube.Value.Item.Id);
            pWriter.WriteLong(cube.Value.Item.Uid);
            pWriter.WriteLong();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.Write(cube.Rotation.Z);
            pWriter.WriteInt();
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet CantPlaceHere(int fieldPlayerObjectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.PlaceFurnishing);
            pWriter.WriteByte(44);
            pWriter.WriteInt(fieldPlayerObjectId);
            pWriter.WriteInt(fieldPlayerObjectId);

            return pWriter;
        }

        public static Packet RemoveCube(int ownerObjectId, int fieldPlayerObjectId, CoordB coord)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.RemoveCube);
            pWriter.WriteByte();
            pWriter.WriteInt(ownerObjectId);
            pWriter.WriteInt(fieldPlayerObjectId);
            pWriter.Write(coord);
            pWriter.WriteByte();
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet RotateCube(IFieldObject<Player> player, IFieldObject<Cube> cube)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.RotateCube);
            pWriter.WriteByte();
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.Write(cube.Coord.ToByte());
            pWriter.WriteByte();
            pWriter.WriteFloat(cube.Rotation.Z);

            return pWriter;
        }

        public static Packet ReplaceCube(int homeOwnerObjectId, int fieldPlayerObjectId, IFieldObject<Cube> newCube, bool sendOnlyObjectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ReplaceCube);
            pWriter.WriteBool(sendOnlyObjectId);
            pWriter.WriteInt(homeOwnerObjectId);
            pWriter.WriteInt(fieldPlayerObjectId);

            if (sendOnlyObjectId)
            {
                return pWriter;
            }
            pWriter.Write(newCube.Coord.ToByte());
            pWriter.WriteByte();
            pWriter.WriteLong(newCube.Value.Item.Uid);
            pWriter.WriteInt(newCube.Value.Item.Id);
            pWriter.WriteLong(newCube.Value.Uid);
            pWriter.WriteLong();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteFloat(newCube.Rotation.Z);
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet Pickup(GameSession session, int weaponId, byte[] coords)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.Pickup);
            pWriter.WriteZero(1);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.Write(coords);
            pWriter.WriteZero(1);
            pWriter.WriteInt(weaponId);
            pWriter.WriteInt(GuidGenerator.Int()); // Item uid

            return pWriter;
        }

        public static Packet Drop(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.Drop);
            pWriter.WriteZero(1);
            pWriter.WriteInt(player.ObjectId);

            return pWriter;
        }

        public static Packet LoadHome(IFieldObject<Player> fieldPlayer)
        {
            Player player = fieldPlayer.Value;

            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.LoadHome);
            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.WriteInt(player.Account.Home?.MapId ?? 0);
            pWriter.WriteInt(player.Account.Home?.PlotId ?? 0);
            pWriter.WriteInt(player.Account.Home?.PlotNumber ?? 0);
            pWriter.WriteInt(player.Account.Home?.ApartmentNumber ?? 0);
            pWriter.WriteUnicodeString(player.Account.Home?.Name ?? "");
            pWriter.WriteLong(player.Account.Home?.Expiration ?? 0);
            pWriter.WriteLong();
            pWriter.WriteByte(1);

            return pWriter;
        }

        public static Packet HomeName(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.HomeName);
            pWriter.WriteByte();
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteInt(player.Account.Home.PlotNumber);
            pWriter.WriteInt(player.Account.Home.ApartmentNumber);
            pWriter.WriteUnicodeString(player.Account.Home.Name);

            return pWriter;
        }

        public static Packet PurchasePlot(int plotNumber, int apartmentNumber, long expiration)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.PurchasePlot);
            pWriter.WriteInt(plotNumber);
            pWriter.WriteInt(apartmentNumber);
            pWriter.WriteByte(1);
            pWriter.WriteLong(expiration);

            return pWriter;
        }

        public static Packet ForfeitPlot(int plotNumber, int apartmentNumber, long expiration)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.PurchasePlot);
            pWriter.WriteInt(plotNumber);
            pWriter.WriteInt(apartmentNumber);
            pWriter.WriteByte(4);
            pWriter.WriteLong(expiration);

            return pWriter;
        }

        public static Packet ChangePassword()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ChangePassword);
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet ArchitectScoreExpiration(long accountId, long now)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ArchitectScoreExpiration);
            pWriter.WriteByte();
            pWriter.WriteLong(accountId);
            pWriter.WriteLong(now);

            return pWriter;
        }

        public static Packet KickEveryone()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.KickEveryone);

            return pWriter;
        }

        public static Packet UpdateArchitectScore(int current, int total)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.UpdateArchitectScore);
            pWriter.WriteInt(current);
            pWriter.WriteInt(total);

            return pWriter;
        }

        public static Packet HomeDescription(string description)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.HomeDescription);
            pWriter.WriteByte();
            pWriter.WriteUnicodeString(description);

            return pWriter;
        }

        public static Packet ReturnMap(int mapId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ReturnMap);
            pWriter.WriteInt(mapId);

            return pWriter;
        }

        public static Packet IncreaseSize(byte size)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.IncreaseSize);
            pWriter.WriteByte();
            pWriter.WriteByte(size);

            return pWriter;
        }

        public static Packet DecreaseSize(byte size)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.DecreaseSize);
            pWriter.WriteByte();
            pWriter.WriteByte(size);

            return pWriter;
        }

        public static Packet EnablePermission(HomePermission permission, bool enabled)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.EnablePermission);
            pWriter.WriteEnum(permission);
            pWriter.WriteBool(enabled);

            return pWriter;
        }

        public static Packet SetPermission(HomePermission permission, byte setting)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.SetPermission);
            pWriter.WriteEnum(permission);
            pWriter.WriteByte(setting);

            return pWriter;
        }

        public static Packet IncreaseHeight(byte height)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.IncreaseHeight);
            pWriter.WriteByte();
            pWriter.WriteByte(height);

            return pWriter;
        }

        public static Packet DecreaseHeight(byte size)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.DecreaseHeight);
            pWriter.WriteByte();
            pWriter.WriteByte(size);

            return pWriter;
        }

        public static Packet ChangeBackground(byte lighthing)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ChangeLighting);
            pWriter.WriteByte();
            pWriter.WriteByte(lighthing);

            return pWriter;
        }

        public static Packet ChangeLighting(byte background)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ChangeBackground);
            pWriter.WriteByte();
            pWriter.WriteByte(background);

            return pWriter;
        }

        public static Packet UpdateBuildingPermissions(long targetAccountId, long ownerAccountId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.GiveBuildingPermission);
            pWriter.WriteLong(targetAccountId);
            pWriter.WriteLong(ownerAccountId);
            pWriter.WriteLong();
            pWriter.WriteLong();

            return pWriter;
        }

        public static Packet ChangeCamera(byte camera)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.ChangeCamera);
            pWriter.WriteByte();
            pWriter.WriteByte(camera);

            return pWriter;
        }

        public static Packet SendWarehouseItems(List<Item> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.LoadWarehouseItems);
            pWriter.WriteShort(3);
            pWriter.WriteInt(items.Count);
            foreach (Item item in items)
            {
                pWriter.WriteInt(item.Id);
                pWriter.WriteInt(item.Amount);
            }

            return pWriter;
        }

        public static Packet AddBuildingPermission(long accountId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.AddBuildingPermission);
            pWriter.WriteByte();
            pWriter.WriteLong(accountId);

            return pWriter;
        }

        public static Packet RemoveBuildingPermission(long accountId, string characterName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.RemoveBuildingPermission);
            pWriter.WriteByte();
            pWriter.WriteLong(accountId);
            pWriter.WriteUnicodeString(characterName);

            return pWriter;
        }

        public static Packet DecorationScore(Home home)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);
            pWriter.WriteEnum(ResponseCubePacketMode.Rewards);
            pWriter.WriteLong(home?.AccountId ?? 0);
            pWriter.WriteLong(home?.DecorationRewardTimestamp ?? 0);
            pWriter.WriteLong(home?.DecorationLevel ?? 1);
            pWriter.WriteLong(home?.DecorationExp ?? 0);
            pWriter.WriteInt(home?.InteriorRewardsClaimed.Count ?? 0);
            if (home != null)
            {
                foreach (int rewardId in home.InteriorRewardsClaimed)
                {
                    pWriter.WriteInt(rewardId);
                }
            }

            return pWriter;
        }
    }
}
