using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class Currency
    {

        private long amount;

        public Currency(long input)
        {
            amount = input;
        }

        public bool Modify(long input)
        {
            if (amount + input < 0)
            {
                return false;
            }
            amount += input;
            return true;
        }

        public void SetAmount(long input)
        {
            if (input < 0)
            {
                return;
            }
            amount = input;
        }

        public long GetAmount()
        {
            return amount;
        }
    }
}
