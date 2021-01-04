using System;
using System.Collections.Generic;
using GameDataParser.Crypto.Stream;

namespace GameDataParser.Crypto.Common
{
    [Serializable]
    public class PackFileEntry : IComparable<PackFileEntry>
    {
        public int Index { get; set; } // The index of the file in the lookup table
        public string Hash { get; set; } // A hash assigned to all files in the directory
        public string Name { get; set; } // The full name of the file (path/name.ext)
        public string TreeName { get; set; } // The visual name displayed in the tree (name.ext)
        public IPackFileHeader FileHeader { get; set; } // The file information (size, offset, etc.)
        public byte[] Data { get; set; } // The raw, decrypted, and current data buffer of the file
        public bool Changed { get; set; } // If the data has been modified in the repacker

        public PackFileEntry CreateCopy(byte[] data = null)
        {
            return new PackFileEntry
            {
                Index = int.MaxValue,
                Hash = this.Hash,
                Name = this.Name,
                TreeName = this.TreeName,
                //FileHeader = this.FileHeader,
                Data = data ?? this.Data,
                Changed = true
            };
        }

        public int CompareTo(PackFileEntry entry)
        {
            if (this.Index == entry.Index)
                return 0;
            if (this.Index > entry.Index)
                return 1;
            return -1;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Hash) ? $"{Index},{Name}\r\n" : $"{Index},{Hash},{Name}\r\n";
        }

        /*
         * Creates a collection of pack file entries from the file string.
         *
         * @param fileString The string containing a table of of files
         *
         * @return A list of file entries with their index/hash/name loaded
         *
        */
        public static List<PackFileEntry> CreateFileList(string fileString)
        {
            List<PackFileEntry> fileList = new List<PackFileEntry>();

            string[] entries = fileString.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string entry in entries)
            {
                int properties = 0;
                foreach (char c in entry)
                {
                    if (c == ',')
                        ++properties;
                }

                string index, name;
                switch (properties)
                {
                    case 1:
                        index = entry.Split(',')[0];
                        name = entry.Split(',')[1];

                        fileList.Add(new PackFileEntry
                        {
                            Index = int.Parse(index),
                            Name = name
                        });
                        break;
                    case 2:
                        index = entry.Split(',')[0];
                        name = entry.Split(',')[2];

                        fileList.Add(new PackFileEntry
                        {
                            Index = int.Parse(index),
                            Hash = entry.Split(',')[1],
                            Name = name
                        });
                        break;
                }
            }

            return fileList;
        }
    }
}
