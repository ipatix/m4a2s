﻿using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;

namespace m4a2s
{
    class Voicegroup
    {
        public static void disassemble(Hashtable index, Entity vgr, string destFile)
        {
            StringBuilder oasm = new StringBuilder();

            oasm.AppendLine("@ File generated by m4a2s Voicegroup-Module");

            oasm.AppendLine("\t.include \"VoiceDef.s\"");
            oasm.AppendLine();
            oasm.AppendLine("\t.section .rodata");
            oasm.AppendLine("\t.global\t" + vgr.Guid);
            oasm.AppendLine("\t.align\t2");
            oasm.AppendLine();
            oasm.AppendLine(vgr.Guid + ":");
            oasm.AppendLine();

            for (int cInstr = 0; cInstr < vgr.VoicegroupLength; cInstr++)
            {
                int valid = IsValidInstrument(vgr.Offset + (cInstr*12));
                if (valid == 0) break;
                if (valid == -1)
                {
                    oasm.AppendLine("@**************** Voice " + cInstr.ToString("D3") + " ****************@");
                    oasm.AppendLine();
                    oasm.AppendLine("\t.byte\tSquareWave1");
                    oasm.AppendLine("\t.byte\tCn3");
                    oasm.AppendLine("\t.byte\t0x00");
                    oasm.AppendLine("\t.byte\t0x00");
                    oasm.AppendLine("\t.word\t" + Tables.WaveDuty[2]);
                    oasm.AppendLine("\t.byte\t0, 0, 15, 0");
                    oasm.AppendLine();

                    continue;
                }
                

                Rom.Reader.BaseStream.Position = vgr.Offset + (cInstr*12);

                oasm.AppendLine("@**************** Voice " + cInstr.ToString("D3") + " ****************@");
                oasm.AppendLine();



                byte instrType = Rom.Reader.ReadByte();
                byte key = Rom.Reader.ReadByte();
                byte par1 = Rom.Reader.ReadByte();
                byte par2 = Rom.Reader.ReadByte();

                int smplPtr;


                switch (instrType)
                {
                    case 0x0:
                        oasm.AppendLine("\t.byte\tDirectSound");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        if (par2 >= 0xC0) oasm.AppendLine("\t.byte\tc_v+" + (par2 - 0xC0));
                        else if (par2 >= 0x80) oasm.AppendLine("\t.byte\tc_v" + (par2 - 0xC0));
                        else oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        smplPtr = Rom.Reader.ReadInt32() - Rom.Map;
                        oasm.AppendLine("\t.word\t" + ((Entity) index[smplPtr]).Guid);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0x8:
                        oasm.AppendLine("\t.byte\tDirectSoundFix");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        if (par2 >= 0xC0) oasm.AppendLine("\t.byte\tc_v+" + (par2 - 0xC0));
                        else if (par2 >= 0x80) oasm.AppendLine("\t.byte\tc_v" + (par2 - 0xC0));
                        else oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        smplPtr = Rom.Reader.ReadInt32() - Rom.Map;
                        oasm.AppendLine("\t.word\t" + ((Entity) index[smplPtr]).Guid);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0x10:
                        oasm.AppendLine("\t.byte\tDirectReverse");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        if (par2 >= 0xC0) oasm.AppendLine("\t.byte\tc_v+" + (par2 - 0xC0));
                        else if (par2 >= 0x80) oasm.AppendLine("\t.byte\tc_v" + (par2 - 0xC0));
                        else oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        smplPtr = Rom.Reader.ReadInt32() - Rom.Map;
                        oasm.AppendLine("\t.word\t" + ((Entity) index[smplPtr]).Guid);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0x18:
                        oasm.AppendLine("\t.byte\tDirectRevFix");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        if (par2 >= 0xC0) oasm.AppendLine("\t.byte\tc_v+" + (par2 - 0xC0));
                        else if (par2 >= 0x80) oasm.AppendLine("\t.byte\tc_v" + (par2 - 0xC0));
                        else oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        smplPtr = Rom.Reader.ReadInt32() - Rom.Map;
                        oasm.AppendLine("\t.word\t" + ((Entity) index[smplPtr]).Guid);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0x1:
                        oasm.AppendLine("\t.byte\tSquareWave1");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        oasm.AppendLine("\t.word\t" + Tables.WaveDuty[Rom.Reader.ReadInt32()]);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0x9:
                        oasm.AppendLine("\t.byte\tASquareWave1");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        oasm.AppendLine("\t.word\t" + Tables.WaveDuty[Rom.Reader.ReadInt32()]);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0x2:
                        oasm.AppendLine("\t.byte\tSquareWave2");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        oasm.AppendLine("\t.word\t" + Tables.WaveDuty[Rom.Reader.ReadInt32()]);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0xA:
                        oasm.AppendLine("\t.byte\tASquareWave2");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        oasm.AppendLine("\t.word\t" + Tables.WaveDuty[Rom.Reader.ReadInt32()]);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0x3:
                        oasm.AppendLine("\t.byte\tProgWave");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        smplPtr = Rom.Reader.ReadInt32() - Rom.Map;
                        oasm.AppendLine("\t.word\t" + ((Entity) index[smplPtr]).Guid);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0xB:
                        oasm.AppendLine("\t.byte\tAProgWave");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        smplPtr = Rom.Reader.ReadInt32() - Rom.Map;
                        oasm.AppendLine("\t.word\t" + ((Entity) index[smplPtr]).Guid);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0x4:
                        oasm.AppendLine("\t.byte\tProgNoise");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        oasm.AppendLine("\t.word\t" + Tables.NoisePattern[Rom.Reader.ReadInt32()]);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0xC:
                        oasm.AppendLine("\t.byte\tAProgNoise");
                        oasm.AppendLine("\t.byte\t" + Tables.Note[key]);
                        oasm.AppendLine("\t.byte\t0x" + par1.ToString("X2"));
                        oasm.AppendLine("\t.byte\t0x" + par2.ToString("X2"));

                        oasm.AppendLine("\t.word\t" + Tables.NoisePattern[Rom.Reader.ReadInt32()]);
                        oasm.AppendLine("\t.byte\t" + Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte() + ", " +
                                        Rom.Reader.ReadByte() + ", " + Rom.Reader.ReadByte());
                        oasm.AppendLine();
                        break;

                    case 0x40:
                        oasm.AppendLine("\t.byte\tKeySplit");
                        oasm.AppendLine("\t.byte\t0x0");
                        oasm.AppendLine("\t.byte\t0x0");
                        oasm.AppendLine("\t.byte\t0x0");

                        int subInstr = Rom.Reader.ReadInt32() - Rom.Map;
                        int keyMap = Rom.Reader.ReadInt32() - Rom.Map;

                        oasm.AppendLine("\t.word\t" + ((Entity) index[subInstr]).Guid);
                        oasm.AppendLine("\t.word\t" + ((Entity) index[keyMap]).Guid);
                        oasm.AppendLine();
                        break;

                    case 0x80:
                        oasm.AppendLine("\t.byte\tDrumTable");
                        oasm.AppendLine("\t.byte\t0x0");
                        oasm.AppendLine("\t.byte\t0x0");
                        oasm.AppendLine("\t.byte\t0x0");

                        int drums = Rom.Reader.ReadInt32() - Rom.Map;

                        oasm.AppendLine("\t.word\t" + ((Entity)index[drums]).Guid);

                        oasm.AppendLine("\t.byte\t0x0, 0x0, 0x0, 0x0");
                        oasm.AppendLine();
                        break;

                }
            } // end instrument loop

            oasm.AppendLine();
            oasm.AppendLine();
            oasm.AppendLine("\t.end");

            File.WriteAllText(destFile, oasm.ToString());
        }

        private static int IsValidInstrument(int offset)
        {
            Rom.Reader.BaseStream.Position = offset;

            byte instrType = Rom.Reader.ReadByte();
            byte key = Rom.Reader.ReadByte();
            byte par1 = Rom.Reader.ReadByte();
            byte par2 = Rom.Reader.ReadByte();

            if (instrType == 0x0 || instrType == 0x8 || instrType == 0x10 || instrType == 0x18)
            {
                // check if the pointer is valid, nothing more to do
                int ptr = Rom.Reader.ReadInt32();
                if (!Index.IsValidPointer(ptr)) return 0;
                if (!Index.GetHashtable().Contains(ptr - Rom.Map)) return -1;
                return 1;
            }
            if (instrType == 0x1 || instrType == 0x2 || instrType == 0x9 || instrType == 0xA)
            {
                int pattern = Rom.Reader.ReadInt32();
                if (pattern < 0 || pattern > 3) return 0;
                byte a = Rom.Reader.ReadByte();
                byte d = Rom.Reader.ReadByte();
                byte s = Rom.Reader.ReadByte();
                byte r = Rom.Reader.ReadByte();
                if (a > 7 || d > 7 || s > 15 || r > 7) return 0;
                return 1;
            }
            if (instrType == 0x3 || instrType == 0xB)
            {
                // check if wave pointer is valid
                int ptr = Rom.Reader.ReadInt32();
                if (!Index.IsValidPointer(ptr)) return 0;
                if (!Index.GetHashtable().Contains(ptr - Rom.Map)) return -1;
                byte a = Rom.Reader.ReadByte();
                byte d = Rom.Reader.ReadByte();
                byte s = Rom.Reader.ReadByte();
                byte r = Rom.Reader.ReadByte();
                if (a > 7 || d > 7 || s > 15 || r > 7) return 0;
                return 1;
            }
            if (instrType == 0x4 || instrType == 0xC)
            {
                int pattern = Rom.Reader.ReadInt32();
                if (pattern < 0 || pattern > 1) return 0;
                byte a = Rom.Reader.ReadByte();
                byte d = Rom.Reader.ReadByte();
                byte s = Rom.Reader.ReadByte();
                byte r = Rom.Reader.ReadByte();
                if (a > 7 || d > 7 || s > 15 || r > 7) return 0;
                return 1;
            }
            if (instrType == 0x40)
            {
                // check if both pointers are valid
                int ptr = Rom.Reader.ReadInt32();
                if (!Index.IsValidPointer(ptr)) return 0;
                if (!Index.GetHashtable().Contains(ptr - Rom.Map)) return -1;
                ptr = Rom.Reader.ReadInt32();
                if (!Index.IsValidPointer(ptr)) return 0;
                if (!Index.GetHashtable().Contains(ptr - Rom.Map)) return -1;
                return 1;
            }
            if (instrType == 0x80)
            {
                // check if pointer is valid and dummy bytes are zero
                int ptr = Rom.Reader.ReadInt32();
                if (!Index.IsValidPointer(ptr)) return 0;
                if (!Index.GetHashtable().Contains(ptr - Rom.Map)) return -1;
                if (Rom.Reader.ReadInt32() != 0) return 0;
                return 1;
            }
            return 0;
        }
    }

    class KeyMap
    {
        public static void disassemble(Entity map, string destFile)
        {
            StringBuilder oasm = new StringBuilder();

            oasm.AppendLine("@ File generated by m4a2s Voicegroup-Module");

            oasm.AppendLine("\t.section .rodata");
            oasm.AppendLine(".global\t" + map.Guid);
            oasm.AppendLine("\t.align\t2");
            oasm.AppendLine();
            oasm.AppendLine(map.Guid + ":");

            Rom.Reader.BaseStream.Position = map.Offset;

            for (int i = 0; i < 128; i++)
            {
                if (i%8 == 0)
                    oasm.Append(Environment.NewLine + "\t.byte\t");
                else oasm.Append("0x" + Rom.Reader.ReadByte().ToString("X2"));
            }

            oasm.AppendLine();
            oasm.AppendLine();
            oasm.AppendLine("\t.end");
        }
    }
}
