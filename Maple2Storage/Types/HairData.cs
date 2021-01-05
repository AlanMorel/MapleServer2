namespace Maple2Storage.Types {
    public struct HairData {
        public float BackLength { get; private set; }
        public float FrontLength { get; private set; }

        public byte[] BackPositionArray { get; private set; }
        public byte[] FrontPositionArray { get; private set; }

        public static HairData hairData(float backLength, float frontLength, byte[] backPositionArray, byte[] frontPositionArray) {
            return new HairData {
                BackLength = backLength,
                FrontLength = frontLength,
                BackPositionArray = backPositionArray,
                FrontPositionArray = frontPositionArray
            };
        }

        public override string ToString() => $"HAIRDATA({BackLength:X2}, {FrontLength:X2}, {BackPositionArray:X2}, {FrontPositionArray:X2})";
    }
}