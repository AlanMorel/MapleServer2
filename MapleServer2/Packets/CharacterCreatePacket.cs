using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ResponseCharCreatePacket
{
    public enum Mode : byte
    {
        SystemError = 0x00, // s_char_err_system
        NameNeeds2LettersMinimum = 0x01, //s_char_err_name
        MaxCharactersReached = 0x06, // s_char_err_char_count
        NameCannotBeUsed = 0x08, // s_char_err_ban_all -- Uses a name that matches exactly a forbidden word
        NameContainsForbiddenWord = 0x09, // s_char_err_ban_any -- Contains a forbidden word
        IncorrectGear = 0x0A, // s_char_err_invalid_def_item
        NameIsTaken = 0x0B, // s_char_err_already_taken
        JobRestriction = 0x0C, // s_char_err_job_forbidden
        AbnormalActivityDetected = 0x0E // s_char_err_creation_restriction
    }

    public static PacketWriter Error(Mode mode, string message = "")
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharCreate);
        pWriter.Write(mode);
        pWriter.WriteUnicodeString(message);

        return pWriter;
    }
}
