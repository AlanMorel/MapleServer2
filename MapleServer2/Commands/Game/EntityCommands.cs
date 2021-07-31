using Maple2Storage.Types;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game
{
    public class NpcCommand : InGameCommand
    {
        public NpcCommand()
        {
            Aliases = new[]
            {
                "npc"
            };
            Description = "Spawn a NPC from id.";
            AddParameter<int>("id", "The id of the NPC.");
            AddParameter<byte>("ani", "The animation of the NPC.");
            AddParameter<short>("dir", "The rotation of the NPC.");
            AddParameter<CoordF>("coord", "The position of the NPC.");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int npcId = trigger.Get<int>("id");

            if (NpcMetadataStorage.GetNpc(npcId) == null)
            {
                trigger.Session.SendNotice($"No NPC was found with the id: <font color='#93f5eb'>{npcId}</font>");
                return;
            }
            Npc npc = new Npc(npcId)
            {
                Animation = trigger.Get<byte>("ani"),
                ZRotation = trigger.Get<short>("dir")
            };

            IFieldObject<Npc> fieldNpc = trigger.Session.FieldManager.RequestFieldObject(npc);
            CoordF coord = trigger.Get<CoordF>("coord");

            if (coord == default)
            {
                fieldNpc.Coord = trigger.Session.FieldPlayer.Coord;
            }
            else
            {
                fieldNpc.Coord = coord;
            }
            trigger.Session.FieldManager.AddNpc(fieldNpc);
        }
    }

    public class MobCommand : InGameCommand
    {
        public MobCommand()
        {
            Aliases = new[]
            {
                "mob"
            };
            Description = "Spawn a MOB from id.";
            AddParameter<int>("id", "The id of the MOB.");
            AddParameter<byte>("ani", "The animation of the MOB.");
            AddParameter<short>("dir", "The rotation of the MOB.");
            AddParameter<CoordF>("coord", "The position of the MOB.");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int mobId = trigger.Get<int>("id");

            if (NpcMetadataStorage.GetNpc(mobId) == null)
            {
                trigger.Session.SendNotice($"No MOB was found with the id: <font color='#93f5eb'>{mobId}</font>");
                return;
            }
            Mob mob = new Mob(mobId)
            {
                Animation = trigger.Get<byte>("ani"),
                ZRotation = trigger.Get<short>("dir")
            };

            IFieldObject<Mob> fieldMob = trigger.Session.FieldManager.RequestFieldObject(mob);
            CoordF coord = trigger.Get<CoordF>("coord");

            if (coord == default)
            {
                fieldMob.Coord = trigger.Session.FieldPlayer.Coord;
            }
            else
            {
                fieldMob.Coord = coord;
            }
            trigger.Session.FieldManager.AddMob(fieldMob);
        }
    }
}
