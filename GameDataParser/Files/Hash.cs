using System;
using System.IO;
using System.Security.Cryptography;

namespace GameDataParser.Files
{
    public static class Hash
    {
        public static bool CheckHash(string filename)
        {
            try
            {
                string currentHash = File.ReadAllText($"{Paths.HASH}/{filename}-hash");

                using (MD5 md5 = MD5.Create())
                {
                    using (FileStream stream = File.OpenRead($"{Paths.OUTPUT}/{filename}"))
                    {
                        byte[] hash = md5.ComputeHash(stream);
                        string newHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

                        return currentHash.Equals(newHash);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.GetType().IsAssignableFrom(typeof(FileNotFoundException)))
                {
                    Console.WriteLine(ex);
                }

                return false;
            }
        }

        public static void WriteHash(string filename)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    using (FileStream stream = File.OpenRead($"{Paths.OUTPUT}/{filename}"))
                    {
                        byte[] hash = md5.ComputeHash(stream);
                        string hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

                        File.WriteAllText($"{Paths.HASH}/{filename}-hash", hashString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
