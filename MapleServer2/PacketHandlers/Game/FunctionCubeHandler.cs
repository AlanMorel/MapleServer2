using Maple2Storage.Enums;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class FunctionCubeHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.FUNCTION_CUBE;

        private enum FunctionCubeMode : byte
        {
            Use = 0x04,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            FunctionCubeMode mode = (FunctionCubeMode) packet.ReadByte();
            switch (mode)
            {
                case FunctionCubeMode.Use:
                    HandleUseCube(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleUseCube(GameSession session, PacketReader packet)
        {
            string coord = packet.ReadUnicodeString();
            byte unk = packet.ReadByte();

            string coordHexa = long.Parse(coord.Split('_')[1]).ToString("X2");
            if (coordHexa.Length == 5)
            {
                coordHexa = "0" + coordHexa;
            }
            CoordB coordB = CoordB.From((sbyte) Convert.ToByte(coordHexa[4..], 16),
                                        (sbyte) Convert.ToByte(coordHexa.Substring(2, 2), 16),
                                        (sbyte) Convert.ToByte(coordHexa.Substring(0, 2), 16));

            IFieldObject<Cube> fieldCube = session.FieldManager.State.Cubes.FirstOrDefault(cube => cube.Value.Coord == coordB.ToFloat()).Value;
            if (fieldCube is null)
            {
                return;
            }

            switch (fieldCube.Value.Item.HousingCategory)
            {
                case ItemHousingCategory.Ranching:
                case ItemHousingCategory.Farming:
                    GatheringHelper.HandleGathering(session, FunctionCubeMetadataStorage.GetRecipeId(fieldCube.Value.Item.Id), out int numDrops);
                    session.FieldManager.BroadcastPacket(FunctionCubePacket.UpdateFunctionCube(coordB, 1, 1));
                    if (numDrops > 0)
                    {
                        session.Send(FunctionCubePacket.SuccessLifeSkill(session.Player.CharacterId, coordB, 1));
                    }
                    else
                    {
                        session.Send(FunctionCubePacket.FailLikeSkill(session.Player.CharacterId, coordB));
                    }
                    session.FieldManager.BroadcastPacket(FunctionCubePacket.UpdateFunctionCube(coordB, 2, 1));
                    break;
                default:
                    Cube cube = fieldCube.Value;
                    cube.InUse = !cube.InUse;

                    session.FieldManager.BroadcastPacket(FunctionCubePacket.UpdateFunctionCube(coordB, cube.InUse ? 1 : 0, 1));
                    session.FieldManager.BroadcastPacket(FunctionCubePacket.UseFurniture(session.Player.CharacterId, coordB, cube.InUse));
                    break;
            }
        }
    }
}
