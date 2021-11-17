using Maple2Storage.Types;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
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

        CoordF coord = trigger.Get<CoordF>("coord");
        if (coord == default)
        {
            coord = trigger.Session.FieldPlayer.Coord;
        }

        trigger.Session.FieldManager.RequestNpc(npcId, coord, CoordF.From(0.0f, 0.0f, trigger.Get<short>("dir")), trigger.Get<short>("ani"));
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

        CoordF coord = trigger.Get<CoordF>("coord");
        if (coord == default)
        {
            coord = trigger.Session.FieldPlayer.Coord;
        }

        trigger.Session.FieldManager.RequestMob(mobId, coord, CoordF.From(0.0f, 0.0f, trigger.Get<short>("dir")), trigger.Get<short>("ani"));
    }
}
