using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace m4a2s
{
    static class Tables
    {
        public static readonly string[] Wxx = // subtract 0x80 before use to rectify index
        {
            "W00", 
            "W01", 
            "W02", 
            "W03", 
            "W04", 
            "W05",
            "W06",
            "W07",
            "W08",
            "W09",
            "W10",
            "W11",
            "W12",
            "W13",
            "W14",
            "W15",
            "W16",
            "W17",
            "W18",
            "W19",
            "W20",
            "W21",
            "W22",
            "W23",
            "W24",
            "W28",
            "W30",
            "W32",
            "W36",
            "W40",
            "W42",
            "W44",
            "W48",
            "W52",
            "W54",
            "W56",
            "W60",
            "W64",
            "W66",
            "W68",
            "W72",
            "W76",
            "W78",
            "W80",
            "W84",
            "W88",
            "W90",
            "W92",
            "W96"
        };

        public static readonly string[] Nxx = // subtract 0xcf before use to rectify index
        { 
            "TIE",
            "N01", 
            "N02", 
            "N03", 
            "N04", 
            "N05",
            "N06",
            "N07",
            "N08",
            "N09",
            "N10",
            "N11",
            "N12",
            "N13",
            "N14",
            "N15",
            "N16",
            "N17",
            "N18",
            "N19",
            "N20",
            "N21",
            "N22",
            "N23",
            "N24",
            "N28",
            "N30",
            "N32",
            "N36",
            "N40",
            "N42",
            "N44",
            "N48",
            "N52",
            "N54",
            "N56",
            "N60",
            "N64",
            "N66",
            "N68",
            "N72",
            "N76",
            "N78",
            "N80",
            "N84",
            "N88",
            "N90",
            "N92",
            "N96"
        };

        public static readonly string[] Note = 
        {
            "CnM2",
            "CsM2", 
            "DnM2", 
            "DsM2",
            "EnM2",
            "FnM2",
            "FsM2",
            "GnM2",
            "GsM2",
            "AnM2",
            "AsM2",
            "BnM2",

            "CnM1",
            "CsM1", 
            "DnM1", 
            "DsM1",
            "EnM1",
            "FnM1",
            "FsM1",
            "GnM1",
            "GsM1",
            "AnM1",
            "AsM1",
            "BnM1",

            "Cn0 ",
            "Cs0 ", 
            "Dn0 ", 
            "Ds0 ",
            "En0 ",
            "Fn0 ",
            "Fs0 ",
            "Gn0 ",
            "Gs0 ",
            "An0 ",
            "As0 ",
            "Bn0 ",

            "Cn1 ",
            "Cs1 ", 
            "Dn1 ", 
            "Ds1 ",
            "En1 ",
            "Fn1 ",
            "Fs1 ",
            "Gn1 ",
            "Gs1 ",
            "An1 ",
            "As1 ",
            "Bn1 ",

            "Cn2 ",
            "Cs2 ", 
            "Dn2 ", 
            "Ds2 ",
            "En2 ",
            "Fn2 ",
            "Fs2 ",
            "Gn2 ",
            "Gs2 ",
            "An2 ",
            "As2 ",
            "Bn2 ",

            "Cn3 ",
            "Cs3 ", 
            "Dn3 ", 
            "Ds3 ",
            "En3 ",
            "Fn3 ",
            "Fs3 ",
            "Gn3 ",
            "Gs3 ",
            "An3 ",
            "As3 ",
            "Bn3 ",

            "Cn4 ",
            "Cs4 ", 
            "Dn4 ", 
            "Ds4 ",
            "En4 ",
            "Fn4 ",
            "Fs4 ",
            "Gn4 ",
            "Gs4 ",
            "An4 ",
            "As4 ",
            "Bn4 ",

            "Cn5 ",
            "Cs5 ", 
            "Dn5 ", 
            "Ds5 ",
            "En5 ",
            "Fn5 ",
            "Fs5 ",
            "Gn5 ",
            "Gs5 ",
            "An5 ",
            "As5 ",
            "Bn5 ",

            "Cn6 ",
            "Cs6 ", 
            "Dn6 ", 
            "Ds6 ",
            "En6 ",
            "Fn6 ",
            "Fs6 ",
            "Gn6 ",
            "Gs6 ",
            "An6 ",
            "As6 ",
            "Bn6 ",

            "Cn7 ",
            "Cs7 ", 
            "Dn7 ", 
            "Ds7 ",
            "En7 ",
            "Fn7 ",
            "Fs7 ",
            "Gn7 ",
            "Gs7 ",
            "An7 ",
            "As7 ",
            "Bn7 ",

            "Cn8 ",
            "Cs8 ", 
            "Dn8 ", 
            "Ds8 ",
            "En8 ",
            "Fn8 ",
            "Fs8 ",
            "Gn8 ",
        };

        public static readonly string[] Memacc =
        {
            "mem_set", "mem_add", "mem_sub", "mem_mem_set",
            "mem_mem_add", "mem_mem_sub", "mem_beq", "mem_bne",
            "mem_bhi", "mem_bhs", "mem_bls", "mem_blo",
            "mem_mem_beq", "mem_mem_bne", "mem_mem_bhi", "mem_mem_bhs",
            "mem_mem_bls", "mem_mem_blo"
        };

        public static readonly string[] Modt =
        {
            "mod_vib", "mod_tre", "mod_pan"
        };

        public static string Gtp(byte gateTime)
        {
            if (gateTime == 0 || gateTime > 3) return gateTime.ToString();
            string[] gtpText = {"gtp1", "gtp2", "gtp3"};
            return gtpText[gateTime - 1];
        }

        public static readonly string[] Xcmd =
        {
            "xIECV", "xIECL"
        };
    }
}
