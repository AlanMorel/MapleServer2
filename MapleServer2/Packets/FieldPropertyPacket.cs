using Maple2.Trigger.Enum;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class FieldPropertyPacket
{
    private enum FieldPropertyMode : byte
    {
        Gravity = 0x01,
        SightRange = 0x06,
        Weather = 0x07,
        AmbientLight = 0x08,
        DirectionalLight = 0x09,
        LocalCamera = 0x0A,
        FreeCamera = 0x0B,
    }

    public static PacketWriter ChangeGravity(float value)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(1);
        pWriter.Write(FieldPropertyMode.Gravity);
        pWriter.WriteFloat(value);

        return pWriter;
    }

    public static PacketWriter ChangeWeather(WeatherType weather)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(1);
        pWriter.Write(FieldPropertyMode.Weather);
        pWriter.WriteByte((byte) weather);

        return pWriter;
    }

    public static PacketWriter ChangeAmbientLight(byte r, byte g, byte b)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(1);
        pWriter.Write(FieldPropertyMode.AmbientLight);
        pWriter.WriteByte(r);
        pWriter.WriteByte(g);
        pWriter.WriteByte(b);

        return pWriter;
    }

    public static PacketWriter ChangeDirectionalLight(byte r, byte g, byte b)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(1);
        pWriter.Write(FieldPropertyMode.DirectionalLight);
        pWriter.WriteByte(r);
        pWriter.WriteByte(g);
        pWriter.WriteByte(b);
        pWriter.WriteByte(); //unknowns
        pWriter.WriteByte();
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter FreeCam(bool enable)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(1);
        pWriter.Write(FieldPropertyMode.FreeCamera);
        pWriter.WriteBool(enable);

        return pWriter;
    }

    public static PacketWriter RangeSight()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(1);
        pWriter.Write(FieldPropertyMode.SightRange);
        pWriter.WriteFloat(900); // radius
        pWriter.WriteFloat(); // these 3 floats control some kind of fade in/out
        pWriter.WriteFloat();
        pWriter.WriteFloat();
        pWriter.WriteByte(); // unknown
        pWriter.WriteByte(75); // opacity of the sight (blackness)
        pWriter.WriteBool(true); // should objects block light?

        return pWriter;
    }

    public static PacketWriter ChangeBackground()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(1);
        pWriter.Write(FieldPropertyMode.SightRange);
        pWriter.WriteFloat(); // radius
        pWriter.WriteFloat(); // these 3 floats control some kind of fade in/out
        pWriter.WriteFloat();
        pWriter.WriteFloat();
        pWriter.WriteByte(); // unknown
        pWriter.WriteByte(); // opacity of the sight (blackness)
        pWriter.WriteBool(true); // should objects block light?

        return pWriter;
    }

    public static PacketWriter SetCharacterInvisible()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(1);
        pWriter.WriteByte(3);

        return pWriter;
    }

    public static PacketWriter SetCharacterVisible()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(2);
        pWriter.WriteByte(3);

        return pWriter;
    }
}
