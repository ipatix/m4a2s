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
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace m4a2s
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("m4a2s version 0.1, Copyright (C) 2015 ipatix");
            Console.WriteLine("m4a2s comes with ABSOLUTELY NO WARRANTY; for details see LICENSE.txt");
            Console.WriteLine("This is free software, and you are welcome to redistribute it");
            Console.WriteLine("under certain conditions; see LICENSE.txt for details.");


            int songtable = 0;
            if (args.Length != 3) ShowUsage();
            if (!File.Exists(args[0])) ShowUsage();
            if (!Directory.Exists(args[2])) ShowUsage();

            try
            {
                songtable = Convert.ToInt32(args[1], 16);
            }
            catch
            {
                ShowUsage();
            }

            string romPath = args[0];
            string destFolder = args[2];


            if (!Directory.Exists(destFolder + "\\seq")) Directory.CreateDirectory(destFolder + "\\seq");
            if (!Directory.Exists(destFolder + "\\wave")) Directory.CreateDirectory(destFolder + "\\wave");
            if (!Directory.Exists(destFolder + "\\bank")) Directory.CreateDirectory(destFolder + "\\bank");
            

            Rom.LoadRom(romPath, songtable);


            /*
             * build up the rom index
             */
            Console.WriteLine("Building up index");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Index.IndexRom();
            sw.Stop();
            Console.WriteLine("Index successfully build in {0} ms", sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();

            Hashtable index = Index.GetHashtable();
            foreach (DictionaryEntry Hent in index)
            {
                Entity ent = (Entity) Hent.Value;

                string fileName;
                switch (ent.Type)
                {
                    case EntityType.Bank:
                        fileName = destFolder + "\\bank\\" + ent.Guid + ".s";
                        Voicegroup.disassemble(index, ent, fileName);
                        break;
                    case EntityType.KeyMap:
                        fileName = destFolder + "\\bank\\" + ent.Guid + ".s";
                        KeyMap.disassemble(ent, fileName);
                        break;
                    case EntityType.Wave:
                        fileName = destFolder + "\\wave\\" + ent.Guid + ".s";
                        Wave.disassemble(ent, fileName);
                        break;
                    case EntityType.GbWave:
                        fileName = destFolder + "\\wave\\" + ent.Guid + ".s";
                        GbWave.disassemble(ent, fileName);
                        break;
                    case EntityType.Song:
                        fileName = destFolder + "\\seq\\" + ent.Guid + ".s";
                        Song.disassemble(index, ent, fileName);
                        break;
                    default:
                        throw new Exception("Invalid Entity Type!");
                }

                Console.WriteLine("Succesfully disassembled Entity from 0x{0} to file: {1}", ent.Offset.ToString("X7"), fileName);
            }


            Songtable.disassemble(index, destFolder + "\\_songtable.s");
            sw.Stop();
            Console.WriteLine("Succesfully disassembled Songtable from 0x{0}", Rom.SongtableOffset);
            Console.WriteLine("Finished exporting data after {0} ms", sw.ElapsedMilliseconds);
        }

        

        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("m4a2s <GBA-ROM.gba> <Songtable-HEX> <Destination-Folder>");
            Environment.Exit(0);
        }
    }
}
