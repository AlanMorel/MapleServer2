using System.Collections.Generic;

namespace Maple2Storage.Helpers
{
    public class StringUtils
    {
        public static int[] SplitStringIntoInts(string list)
        {
            string[] split = list.Split(new char[1] {','});
            List<int> numbers = new List<int>();

            foreach (string n in split)
            {
                if (int.TryParse(n, out int parsed))
                {
                    numbers.Add(parsed);
                }
            }

            return numbers.ToArray();
        }
    }
}
