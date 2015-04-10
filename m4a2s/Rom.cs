/*
 * m4a2s is aprogram to to dump sound files from m4a/mp2k GBA games to pseudo-assembly source files
 * Copyright (C) 2015 ipatix
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA
 */

using System;
using System.IO;

namespace m4a2s
{
    static class Rom
    {
        public static int SongtableOffset { get; private set; }
        public static int NumSongs { get; set; }
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

        public static int ReaderPeekByte()
        {
            long org = Reader.BaseStream.Position;
            byte ret = Reader.ReadByte();
            Reader.BaseStream.Position = org;
            return ret;
        }
    }
}
