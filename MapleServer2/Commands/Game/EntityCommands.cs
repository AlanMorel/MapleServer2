using Maple2Storage.Types;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;
using Color = System.Drawing.Color;

namespace MapleServer2.Commands.Game;

public class NpcCommand : InGameCommand
{
    public NpcCommand()
    {
        Aliases = new()
        {
            "npc"
        };
        Description = "Spawn a NPC from id.";
        Parameters = new()
        {
            new Parameter<int>("id", "The id of the NPC.", 11003146),
            new Parameter<short>("ani", "The animation of the NPC.", 1),
            new Parameter<short>("dir", "The rotation of the NPC.", 2700),
            new Parameter<CoordF>("coord", "The position of the NPC.", CoordF.From(0, 0, 0))
        };
        Usage = "/npc [id] [ani] [dir] [coord]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int npcId = trigger.Get<int>("id");

        if (NpcMetadataStorage.GetNpcMetadata(npcId) is null)
        {
            trigger.Session.Send(NoticePacket.Notice($"No NPC was found with the id: {npcId.ToString().Color(Color.DarkOliveGreen)}", NoticeType.Chat));
            return;
        }
        Npc npc = new(npcId)
        {
            Animation = trigger.Get<short>("ani"),
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
        Aliases = new()
        {
            "mob"
        };
        Description = "Spawn a MOB from id.";
        Parameters = new()
        {
            new Parameter<int>("id", "The id of the MOB.", 21000001),
            new Parameter<short>("ani", "The animation of the MOB.", 1),
            new Parameter<short>("dir", "The rotation of the MOB.", 2700),
            new Parameter<CoordF>("coord", "The position of the MOB.", CoordF.From(0, 0, 0))
        };
        Usage = "/mob [id] [ani] [dir] [coord]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int mobId = trigger.Get<int>("id");

        if (NpcMetadataStorage.GetNpcMetadata(mobId) is null)
        {
            trigger.Session.Send(NoticePacket.Notice($"No MOB was found with the id: {mobId.ToString().Color(Color.DarkOliveGreen)}", NoticeType.Chat));
            return;
        }
        Mob mob = new(mobId)
        {
            Animation = trigger.Get<short>("ani"),
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
