using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m4a2s
{
    class Voicegroup
    {
        public static void disassemble(Hashtable index, Entity vgr, string destFile)
        {
            int vgrOffset = vgr.Offset;

            StringBuilder oasm = new StringBuilder();

            for (int cInstr = 0; cInstr < vgr.VoicegroupLength: cInstr++)
            {
                Rom.Reader.BaseStream.Position = vgr.Offset + (cInstr*12);

                uint instrType = Rom.Reader.ReadUint32();

                switch (instrType & 0xFF)
                {
                    case 0x0:
                    case 0x8:
                    case 0x10:
                    case 0x18:
                        break;
                    case 0x1:
                    case 0x9:
                    case 0x2:
                    case 0xA:
                    case 0x3:
                    case 0xB:
                        break;
                    case 0x3:
                    case 0xB:
                        break;
                    case 0x40:
                        break;
                    case 0x80:
                        break;
                }
            }
        }
    }
}
