﻿using MapleServer2.Commands.Core;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game;

public class EventCommands : InGameCommand
{
    public EventCommands()
    {
        Aliases = new()
        {
            "event"
        };
        Description = "Create a global event";
        Parameters = new()
        {
            new Parameter<string[]>("id", "Event Id")
        };
        Usage = "/event [id],[id],[id]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string[] args = trigger.Get<string[]>("id");

        if (args == null || args.Length <= 1)
        {
            trigger.Session.Send(NoticePacket.Notice("No events provided. Choose one through three events. \n" +
                                                     "1 = OX Quiz \n" +
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
                                                     "13 = Christmas Dance Dance Stop", NoticeType.Chat));
            return;
        }

        GlobalEvent globalEvent = new();

        byte[] eventIds = Array.ConvertAll(args[1].Split(","), byte.Parse);
        if (eventIds.Length > 3)
        {
            trigger.Session.Send(NoticePacket.Notice("Too many events chosen. Please choose only between one and three.", NoticeType.Chat));
            return;
        }

        foreach (byte eventId in eventIds)
        {
            if (Enum.IsDefined(typeof(GlobalEventType), eventId))
            {
                globalEvent.Events.Add((GlobalEventType) eventId);
            }
        }

        if (globalEvent.Events.Count == 1)
        {
            globalEvent.Events.Insert(0, GlobalEventType.none);
            globalEvent.Events.Insert(2, GlobalEventType.none);

        }
        else if (globalEvent.Events.Count == 2)
        {
            globalEvent.Events.Insert(2, GlobalEventType.none);
        }

        globalEvent.Start();
    }
}
