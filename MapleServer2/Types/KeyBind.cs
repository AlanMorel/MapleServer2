using System.Runtime.InteropServices;

namespace MapleServer2.Types {
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 21)]
    public struct KeyBind {
        public int KeyCode { get; private set; }
        public int OptionType { get; private set; }
        public long OptionGuid { get; private set; }
        // Haven't found a non-zero value for this
        public int Unknown1 { get; private set; }
        public byte Priority { get; private set; }

        public static KeyBind From(int keyCode, int optionType, long optionGuid, byte priority, int unknown1 = 0) {
            return new KeyBind {
                KeyCode = keyCode,
                OptionType = optionType,
                OptionGuid = optionGuid,
                Unknown1 = unknown1,
                Priority = priority,
            };
        }

        public override string ToString() => $"KeyBind({KeyCode}, {OptionType}, {OptionGuid}, {Unknown1}, {Priority}";
    }
}
