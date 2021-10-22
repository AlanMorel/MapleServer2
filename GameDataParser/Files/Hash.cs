using System.Security.Cryptography;
using Maple2Storage.Types;

namespace GameDataParser.Files
{
    public static class Hash
    {
        public static bool HasValidHash(string filename)
        {
            string hashPath = $"{Paths.HASH_DIR}/{filename}-hash";

            if (!File.Exists(hashPath))
            {
                return false;
            }

            string currentHash = File.ReadAllText(hashPath);
            string newHash = GetHash(filename);

            return currentHash.Equals(newHash);
        }

        public static void WriteHash(string filename)
        {
            string hashPath = $"{Paths.HASH_DIR}/{filename}-hash";

            string newHash = GetHash(filename);

            File.WriteAllText(hashPath, newHash);
        }

        public static string GetHash(string filename)
        {
            string filepath = $"{Paths.RESOURCES_DIR}/{filename}";

            if (!File.Exists(filepath))
            {
                return "";
            }

            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filepath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
