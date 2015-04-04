using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace m4a2s
{
    static class Rom
    {
        public static int SongtableOffset { get; private set; }
        private static byte[] _rom;
        public static MemoryStream RomStream { get; private set; }
        public static BinaryReader Reader { get; private set; }

        public const int Map = 0x8000000;

        public static void LoadRom(string filePath, int songtableOffset)
        {
            _rom = File.ReadAllBytes(filePath);
            if (songtableOffset >= _rom.Length || (songtableOffset % 4) != 0) throw new Exception("Invalid Songtable location");
            
            SongtableOffset = songtableOffset;
            RomStream = new MemoryStream(_rom);
            Reader = new BinaryReader(RomStream);
        }

        public static long RomSize
        {
            get { return _rom.Length; }
        }
    }
}
