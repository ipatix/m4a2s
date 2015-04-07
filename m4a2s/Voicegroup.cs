using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace m4a2s
{
    class Voicegroup
    {
        public static void disassemble(Hashtable index, Entity vgr, string destFile)
        {
            int vgrOffset = vgr.Offset;

            StringBuilder oasm = new StringBuilder();

            oasm.AppendLine("\t.include \"VoiceDef.s\"");
            oasm.AppendLine();
            oasm.AppendLine("\t.section .rodata");
            oasm.AppendLine(".global\t" + vgr.Guid);
            oasm.AppendLine("\t.align\t2");
            oasm.AppendLine();
            oasm.AppendLine(vgr.Guid + ":");
            oasm.AppendLine();

            for (int cInstr = 0; cInstr < vgr.VoicegroupLength; cInstr++)
            {
                Rom.Reader.BaseStream.Position = vgr.Offset + (cInstr*12);

                oasm.AppendLine("@**************** Voice " + cInstr.ToString("D3") + " ****************@");
                oasm.AppendLine();



                byte instrType = Rom.Reader.ReadByte();
                byte key = Rom.Reader.ReadByte();
                byte par1 = Rom.Reader.ReadByte();
                byte par2 = Rom.Reader.ReadByte();

                int smplPtr = 0;


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

                        int subInstr = Rom.Reader.ReadInt32();
                        int keyMap = Rom.Reader.ReadInt32();

                        oasm.AppendLine("\t.word\t" + ((Entity) index[subInstr]).Guid);
                        oasm.AppendLine("\t.word\t" + ((Entity) index[keyMap]).Guid);
                        oasm.AppendLine();
                        break;

                    case 0x80:
                        oasm.AppendLine("\t.byte\tDrumTable");
                        oasm.AppendLine("\t.byte\t0x0");
                        oasm.AppendLine("\t.byte\t0x0");
                        oasm.AppendLine("\t.byte\t0x0");

                        int drums = Rom.Reader.ReadInt32();

                        oasm.AppendLine("\t.word\t" + ((Entity)index[drums]).Guid);

                        oasm.AppendLine("\t.byte\t0x0, 0x0, 0x0, 0x0");
                        break;

                }
            } // end instrument loop

            oasm.AppendLine();
            oasm.AppendLine();
            oasm.AppendLine("\t.end");
        }
    }
}
