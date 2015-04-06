using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m4a2s
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2) ShowUsage();
            if (!System.IO.File.Exists(args[0])) ShowUsage();

            int songtable = 0;

            try
            {
                songtable = Convert.ToInt32(args[1], 16);
            }
            catch (Exception e)
            {
                ShowUsage();
            }

            Rom.LoadRom(args[1], songtable);

        }

        

        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("m4a2s <GBA-ROM.gba> <Songtable-HEX>");
            Environment.Exit(0);
        }
    }
}
