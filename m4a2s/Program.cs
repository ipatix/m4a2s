using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m4a2s
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3) ShowUsage();
            if (!System.IO.File.Exists(args[0])) ShowUsage();
            if (!System.IO.Directory.Exists(args[2])) ShowUsage();

            int songtable = 0;

            try
            {
                songtable = Convert.ToInt32(args[1], 16);
            }
            catch (Exception e)
            {
                ShowUsage();
            }

            string romPath = args[0];
            string destFolder = args[2];

            if (Directory.Exists(destFolder + "\\seq")) Directory.CreateDirectory(destFolder + "\\seq");
            if (Directory.Exists(destFolder + "\\wave")) Directory.CreateDirectory(destFolder + "\\wave");
            if (Directory.Exists(destFolder + "\\bank")) Directory.CreateDirectory(destFolder + "\\bank");
            

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
            foreach (Entity ent in index)
            {
                switch (ent.Type)
                {
                        case EntityType.Bank:

                        break;
                        case EntityType.GbWave:
                        break;
                        case EntityType.KeyMap:
                        break;
                        case EntityType.Song:
                        break;
                        case EntityType.Wave:

                }
            }
        }

        

        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("m4a2s <GBA-ROM.gba> <Songtable-HEX>");
            Environment.Exit(0);
        }
    }
}
