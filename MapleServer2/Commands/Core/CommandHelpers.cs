using System.Text;
using Maple2Storage.Types;

namespace MapleServer2.Commands.Core
{
    public static class CommandHelpers
    {
        internal static void TryParseCoord(int index, string[] args, out CoordF result)
        {
            float[] coord = new float[3];
            coord[0] = float.TryParse(args[index + 1], out float x) ? x : 0;
            coord[1] = float.TryParse(args[index + 2], out float y) ? y : 0;
            coord[2] = float.TryParse(args[index + 3], out float z) ? z : 0;
            result = CoordF.From(coord[0], coord[1], coord[2]);
        }

        internal static string BuildString(string[] args)
        {
            StringBuilder stringBuilder = new();

            foreach (string arg in args)
            {
                stringBuilder.Append($"{arg} ");
            }
            return stringBuilder.ToString();
        }
    }
}
