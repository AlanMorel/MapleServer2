using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public class HomeActionPacket
{
    private enum HomeActionMode : byte
    {
        Alarm = 0x04,
        Survey = 0x05,
        PortalCube = 0x06,
        Ball = 0x07,
        Roll = 0x0B
    }

    public static PacketWriter SendCubePortalSettings(Cube cube, List<Cube> otherPortals)
    {
        CubePortalSettings portalSettings = cube.PortalSettings;

        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);

        pWriter.Write(HomeActionMode.PortalCube);
        pWriter.WriteByte();
        pWriter.Write(cube.CoordF.ToByte());
        pWriter.WriteByte();
        pWriter.WriteUnicodeString(portalSettings.PortalName);
        pWriter.Write(portalSettings.Method);
        pWriter.Write(portalSettings.Destination);
        pWriter.WriteUnicodeString(portalSettings.DestinationTarget ?? "");
        pWriter.WriteInt(otherPortals.Count);
        foreach (Cube otherPortal in otherPortals)
        {
            pWriter.WriteUnicodeString(otherPortal.PortalSettings.PortalName);
        }

        return pWriter;
    }


    public static PacketWriter HostAlarm(string message)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Alarm);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteUnicodeString(message);

        return pWriter;
    }

    public static PacketWriter SurveyMessage()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Survey);
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter SurveyQuestion(HomeSurvey survey)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Survey);
        pWriter.WriteByte(2);
        pWriter.WriteUnicodeString(survey.Question);
        pWriter.WriteBool(survey.Public);

        return pWriter;
    }

    public static PacketWriter SurveyAddOption(HomeSurvey survey)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Survey);
        pWriter.WriteByte(3);
        pWriter.WriteUnicodeString(survey.Question);
        pWriter.WriteBool(survey.Public);
        pWriter.WriteUnicodeString(survey.Options.Keys.Last());
        pWriter.WriteByte(1);

        return pWriter;
    }

    public static PacketWriter SurveyStart(HomeSurvey survey)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Survey);
        pWriter.WriteByte(4);
        pWriter.WriteLong(survey.OwnerId); // character id
        pWriter.WriteLong(survey.Id); // unk
        pWriter.WriteBool(survey.Public);
        pWriter.WriteUnicodeString(survey.Question);
        pWriter.WriteByte((byte) survey.Options.Count);
        foreach (string option in survey.Options.Keys)
        {
            pWriter.WriteUnicodeString(option);
        }

        return pWriter;
    }

    public static PacketWriter SurveyAnswer(string name)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Survey);
        pWriter.WriteByte(5);
        pWriter.WriteUnicodeString(name);

        return pWriter;
    }

    public static PacketWriter SurveyEnd(HomeSurvey survey)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Survey);
        pWriter.WriteByte(6);
        pWriter.WriteLong(survey.Id);
        pWriter.WriteBool(survey.Public);
        pWriter.WriteUnicodeString(survey.Question);
        pWriter.WriteByte((byte) survey.Options.Count);
        foreach ((string option, List<string> characterNames) in survey.Options)
        {
            pWriter.WriteUnicodeString(option);
            pWriter.WriteByte((byte) characterNames.Count);

            if (!survey.Public)
            {
                continue;
            }

            foreach (string characterName in characterNames)
            {
                pWriter.WriteUnicodeString(characterName);
            }
        }

        pWriter.WriteByte((byte) survey.AvailableCharacters.Count);
        if (survey.Public)
        {
            foreach (string characterName in survey.AvailableCharacters)
            {
                pWriter.WriteUnicodeString(characterName);
            }
        }

        return pWriter;
    }

    public static PacketWriter AddBall(IFieldObject<GuideObject> guide)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Ball);
        pWriter.WriteByte();
        pWriter.WriteInt(guide.ObjectId);
        pWriter.WriteLong(guide.Value.BoundCharacterId);
        pWriter.Write(guide.Coord);
        pWriter.WriteFloat(guide.Rotation.Z);

        return pWriter;
    }

    public static PacketWriter RemoveBall(IFieldObject<GuideObject> guide)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Ball);
        pWriter.WriteByte(1);
        pWriter.WriteInt(guide.ObjectId);
        
        return pWriter;
    }

    public static PacketWriter UpdateBall(IFieldObject<GuideObject> guide, CoordF velocity1, CoordF velocity2)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Ball);
        pWriter.WriteByte(2);
        pWriter.WriteInt(guide.ObjectId);
        pWriter.WriteLong(guide.Value.BoundCharacterId);
        pWriter.Write(guide.Coord);
        pWriter.Write(velocity1);
        pWriter.Write(velocity2);

        return pWriter;
    }

    public static PacketWriter HitBall(IFieldObject<GuideObject> guide, CoordF velocity1)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Ball);
        pWriter.WriteByte(3);
        pWriter.WriteInt(guide.ObjectId);
        pWriter.WriteLong(guide.Value.BoundCharacterId);
        pWriter.Write(guide.Coord);
        pWriter.Write(velocity1);

        return pWriter;
    }

    public static PacketWriter Roll(Player player, int randomNumber)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);
        pWriter.Write(HomeActionMode.Roll);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteByte();
        pWriter.WriteByte(1);
        pWriter.WriteInt(1);
        pWriter.Write(SystemNotice.RandomNumberSelection);
        pWriter.WriteInt(2); // number of strings
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteUnicodeString(randomNumber.ToString());

        return pWriter;
    }
}
