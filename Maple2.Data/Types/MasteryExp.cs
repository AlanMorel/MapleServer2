using System.Runtime.InteropServices;

namespace Maple2.Data.Types {
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 48)]
    public struct MasteryExp {
        public int Unknown;
        public int Fishing;
        public int Instrument;
        public int Mining;
        public int Foraging;
        public int Ranching;
        public int Farming;
        public int Smithing;
        public int Handicrafts;
        public int Alchemy;
        public int Cooking;
        public int PetTaming;
    }
}