using System;
using System.IO;
using System.Resources;

namespace GameDataParser.Crypto.Common
{
    public class MultiArrayResource : IMultiArray
    {
        private readonly ResourceManager resourceManager;
        private readonly string resourceName;
        private readonly Lazy<byte[][]> lazyResource; // TODO: maybe array of lazy (Lazy<byte[]>[])

        string IMultiArray.Name => this.resourceName;
        public int ArraySize { get; }
        public int Count { get; }

        public byte[] this[uint index] => this.lazyResource.Value[index % this.Count];

        public MultiArrayResource(ResourceManager resourceManager, string resourceName, int count, int arraySize)
        {
            this.resourceManager = resourceManager;
            this.resourceName = resourceName;
            this.Count = count;
            this.ArraySize = arraySize;

            this.lazyResource = new Lazy<byte[][]>(this.CreateLazyImplementation);
        }

        private byte[][] CreateLazyImplementation()
        {
            byte[][] result = new byte[this.Count][];

            byte[] data = (byte[]) this.resourceManager.GetObject(this.resourceName);
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                for (int i = 0; i < this.Count; i++)
                {
                    byte[] bytes = reader.ReadBytes(this.ArraySize);
                    if (bytes.Length == this.ArraySize)
                    {
                        result[i] = bytes;
                    }
                }
            }

            return result;
        }
    }
}
