namespace MapleServer2.Types
{
    public class Pet
    {
        public readonly int Id;
        public readonly long Uid;

        public Pet(int id, long uid)
        {
            Id = id;
            Uid = uid;
        }
    }
}
