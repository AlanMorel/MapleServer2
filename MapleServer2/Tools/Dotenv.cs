﻿using System;
using System.IO;

namespace MapleServer2.Tools
{
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    continue;
                }

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}
