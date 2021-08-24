using MapleServer2.Commands.Core;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game
{
    public class EventCommands : InGameCommand
    {
        public EventCommands()
        {
            Aliases = new[]
            {
                "event"
            };
            Description = "Create a global event";
            AddParameter<string[]>("id", "Event Id");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            string[] args = trigger.Get<string[]>("id");

            if (args == null || args.Length <= 1)
            {
                trigger.Session.SendNotice("No events provided. Choose 3 events. 1 = OX Quiz \n" +
                    "2 = Trap Master \n" +
                    "3 = Spring Beach \n" +
                    "4 = Crazy Runner \n" +
                    "5 = Final Survivor \n" +
                    "6 = Ludibrium Escape \n" +
                    "7 = Dance Dance Stop \n" +
                    "8 = Crazy Runner Shanghai \n" +
                    "9 = Hide And Seek \n" +
                    "10 = Red Arena \n" +
                    "11 = Blood Mine \n" +
                    "12 = Treasure Island \n" +
                    "13 = Christmas Dance Dance Stop");
                return;
            }

            GlobalEvent globalEvent = new GlobalEvent();

            byte[] eventIds = Array.ConvertAll(args[1].Split(","), byte.Parse);
            foreach (byte eventId in eventIds)
            {
                if (Enum.IsDefined(typeof(GlobalEventType), eventId))
                {
                    globalEvent.Events.Add((GlobalEventType) eventId);
                }
            }
            GameServer.GlobalEventManager.AddEvent(globalEvent);

            _ = globalEvent.Start();
        }
    }
}
