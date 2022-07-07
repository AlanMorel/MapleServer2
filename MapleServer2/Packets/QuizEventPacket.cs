using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class QuizEventPacket
{
    private enum Mode : byte
    {
        Question = 0x0,
        Answer = 0x1
    }

    public static PacketWriter Question(string category, string question, int duration)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QuizEvent);
        pWriter.Write(Mode.Question);
        pWriter.WriteUnicodeString(category);
        pWriter.WriteUnicodeString(question);
        pWriter.WriteUnicodeString();
        pWriter.WriteInt(duration);
        return pWriter;
    }

    public static PacketWriter Answer(bool answer, string answerText, int duration)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QuizEvent);
        pWriter.Write(Mode.Answer);
        pWriter.WriteBool(answer);
        pWriter.WriteUnicodeString(answerText);
        pWriter.WriteInt(duration);
        return pWriter;
    }
}
