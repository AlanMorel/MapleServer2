using System;
using System.IO;
using System.Resources;

namespace GameDataParser.Crypto.Common
{
    public class MultiArrayResource : IMultiArray
    {
        private readonly ResourceManager ResourceManager;
        private readonly string ResourceName;
        private readonly Lazy<byte[][]> LazyResource; // TODO: maybe array of lazy (Lazy<byte[]>[])

        string IMultiArray.Name => ResourceName;
        public int ArraySize { get; }
        public int Count { get; }

        public byte[] this[uint index] => LazyResource.Value[index % Count];

        public MultiArrayResource(ResourceManager resourceManager, string resourceName, int count, int arraySize)
        {
            ResourceManager = resourceManager;
            ResourceName = resourceName;
            Count = count;
            ArraySize = arraySize;

            LazyResource = new Lazy<byte[][]>(CreateLazyImplementation);
        }

        private byte[][] CreateLazyImplementation()
        {
            byte[][] result = new byte[Count][];

            byte[] data = (byte[]) ResourceManager.GetObject(ResourceName);
            using (BinaryReader reader = new BinaryReader(new MemoryStream(data)))
            {
                for (int i = 0; i < Count; i++)
                {
                    byte[] bytes = reader.ReadBytes(ArraySize);
                    if (bytes.Length == ArraySize)
                    {
                        result[i] = bytes;
                    }
                }
            }

            return result;
        }
    }
}
