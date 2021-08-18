namespace MapleServer2.Tools
{
    public static class GuidGenerator
    {
        // Generate a 64 bit Guid
        public static long Long()
        {
            return Math.Abs(BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0));
        }

        // Generate a 32 bit Guid
        public static int Int()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }
    }
}
