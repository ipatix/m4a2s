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
            int songtable = 0x4A32CC;
            /*if (args.Length != 3) ShowUsage();
            if (!File.Exists(args[0])) ShowUsage();
            if (!Directory.Exists(args[2])) ShowUsage();

            try
            {
                songtable = Convert.ToInt32(args[1], 16);
            }
            catch
            {
                ShowUsage();
            }*/

            string romPath = "Pokemon Fire Red Version (U).gba";//args[0];
            string destFolder = ".";//args[2];


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

            Hashtable index = Index.GetHashtable();
            foreach (DictionaryEntry Hent in index)
            {
                Entity ent = (Entity) Hent.Value;

                string fileName;
                switch (ent.Type)
                {
                    case EntityType.Bank:
                    case EntityType.KeyMap:
                        fileName = destFolder + "\\bank\\" + ent.Guid + ".s";
                        Voicegroup.disassemble(index, ent, fileName);
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
            Console.WriteLine("Succesfully disassembled Songtable from 0x{0}", Rom.SongtableOffset);
        }

        

        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("m4a2s <GBA-ROM.gba> <Songtable-HEX>");
            Environment.Exit(0);
        }
    }
}
