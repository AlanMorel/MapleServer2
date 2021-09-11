﻿using Maple2Storage.Extensions;

namespace Maple2Storage.Tools
{
    public static class ConsoleUtility
    {
        private const char Block = '■';

        public static void WriteProgressBar(float percent)
        {
            Console.Write(" [");
            for (int i = 0; i < 10; i++)
            {
                if (i >= percent / 10)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write(Block);
                }
            }

            if (percent <= 50f)
            {
                Console.Write("]");
                Console.Write(" {0,1:0}%\r".ColorRed(), percent);
            }

            else if (percent <= 100f)
            {
                Console.Write("]");
                Console.Write(" {0,1:0}%\r".ColorYellow(), percent);
            }
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }
    }
}
