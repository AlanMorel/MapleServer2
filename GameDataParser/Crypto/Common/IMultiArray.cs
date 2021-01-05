namespace GameDataParser.Crypto.Common {
    public interface IMultiArray {
        string Name { get; }
        int ArraySize { get; }
        int Count { get; }

        byte[] this[uint index] { get; }
    }
}