using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace m4a2s
{
    abstract class SongEvent
    {
        public int EventOffset { get; private set; }

        protected SongEvent(int eventOffset)
        {
            EventOffset = eventOffset;
        }

        public abstract override string ToString();
    }

    class DelaySongEvent : SongEvent
    {
        public byte DelayLength { get; private set; }   // important note: This doesn't contain the actual delay values. It contains the delay index for the length lookup table

        public DelaySongEvent(int eventOffset, byte delayLength) : base(eventOffset)
        {
            if (delayLength > 0x30) throw new IndexOutOfRangeException("The delay was out of expected range at DelaySongEvent() call");
            DelayLength = delayLength;
        }

        public override string ToString()
        {
            return "\t.byte\t" + Tables.Wxx[DelayLength] + Environment.NewLine;
        }
    }

    class FineSongEvent : SongEvent
    {
        public FineSongEvent(int eventOffset) : base(eventOffset) { }

        public override string ToString()
        {
            return "\t.byte\tFINE" + Environment.NewLine;
        }
    }

    class GotoSongEvent : SongEvent
    {
        public int TargetOffset { get; private set; }

        public GotoSongEvent(int eventOffset, int targetOffset) : base(eventOffset)
        {
            TargetOffset = targetOffset;
        }

        public override string ToString()
        {
            return "\t.byte\tGOTO" + Environment.NewLine + "\t .word\tloop_" + TargetOffset.ToString("X8") + Environment.NewLine;
        }
        public string ToString(string symbolName)
        {
            return "\t.byte\tGOTO" + Environment.NewLine + "\t .word\t" + symbolName + Environment.NewLine;
        }
    }

    class PattSongEvent : SongEvent
    {
        public int TargetOffset { get; private set; }

        public PattSongEvent(int eventOffset, int targetOffset) : base(eventOffset)
        {
            TargetOffset = targetOffset;
        }

        public override string ToString()
        {
            return "\t.byte\tPATT" + Environment.NewLine + "\t .word\tpatt_" + TargetOffset.ToString("X8") +
                   Environment.NewLine;
        }
        public string ToString(string symbolName)
        {
            return "\t.byte\tPATT" + Environment.NewLine + "\t .word\t" + symbolName +
                   Environment.NewLine;
        }
    }

    class PendSongEvent : SongEvent
    {
        public PendSongEvent(int eventOffset) : base(eventOffset) { }

        public override string ToString()
        {
            return "\t.byte\tPEND" + Environment.NewLine;
        }
    }

    class ReptSongEvent : SongEvent
    {
        public int TargetOffset { get; private set; }
        public byte RepeatCount { get; private set; }

        public ReptSongEvent(int eventOffset, int targetOffset, byte repeatCount) : base(eventOffset)
        {
            TargetOffset = targetOffset;
            RepeatCount = repeatCount;
        }

        public override string ToString()
        {
            return "\t.byte\tREPT ,\t" + RepeatCount + Environment.NewLine + "\t .word\tpatt_" +
                   TargetOffset.ToString("X8") + Environment.NewLine;
        }
        public string ToString(string symbolName)
        {
            return "\t.byte\tREPT ,\t" + RepeatCount + Environment.NewLine + "\t .word\t" + symbolName + Environment.NewLine;
        }
    }

    class MemAccEvent : SongEvent
    {
        public byte Par1 { get; private set; }
        public byte Par2 { get; private set; }
        public byte Par3 { get; private set; }

        public MemAccEvent(int eventOffset, byte par1, byte par2, byte par3) : base(eventOffset)
        {
            Par1 = par1;
            Par2 = par2;
            Par3 = par3;
        }

        public override string ToString()
        {
            string returnValue = "\t.byte\tMEMACC" + Environment.NewLine + "\t.byte\t";
            if (Par1 > 17) returnValue += Par1 + " ,  ";
            else returnValue += Tables.Memacc[Par1] + " ,  ";
            returnValue += Par2 + " ,  " + Par3 + Environment.NewLine;
            return returnValue;
        }
    }

    class PrioSongEvent : SongEvent
    {
        public byte Priority { get; private set; }

        public PrioSongEvent(int eventOffset, byte prio) : base(eventOffset)
        {
            Priority = prio;
        }

        public override string ToString()
        {
            return "\t.byte\tPRIO  , " + Priority + Environment.NewLine;
        }
    }

    class TempoSongEvent : SongEvent
    {
        public byte Tempo { get; private set; }

        public TempoSongEvent(int eventOffset, byte tempo) : base(eventOffset)
        {
            Tempo = tempo;
        }

        public string ToString(string symbolName)
        {
            return "\t.byte\tTEMPO , " + (Tempo*2) + "*" + symbolName + "_tbs/2" + Environment.NewLine;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }

    class KeyshSongEvent : SongEvent
    {
        public sbyte Transpose { get; private set; }

        public KeyshSongEvent(int eventOffset, sbyte transpose) : base(eventOffset)
        {
            Transpose = transpose;
        }

        public string ToString(string symbolName)
        {
            return "\t.byte\tKEYSH , " + symbolName + "_key" + Transpose.ToString("+00;-00;+00");
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }

    class VoiceSongEvent : SongEvent
    {
        public byte InstrNum { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public VoiceSongEvent(int eventOffset, byte instrNum, bool repeatedEvent) : base(eventOffset)
        {
            if (instrNum > 127) throw new ArgumentException("instrNum > 127 at VoiceSongEvent()");
            InstrNum = instrNum;
            RepeatedEvent = repeatedEvent;
        }

        public override string ToString()
        {
            if (RepeatedEvent) return "\t.byte\t\t        " + InstrNum + Environment.NewLine;
            return "\t.byte\t\tVOICE , " + InstrNum + Environment.NewLine;
        }
    }

    class VolSongEvent : SongEvent
    {
        public byte Volume { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public VolSongEvent(int eventOffset, byte volume) : base(eventOffset)
        {
            if (Volume > 127) throw  new ArgumentException("volume > 127 at VolSongEvent()");
            Volume = volume;
        }

        public override string ToString()
        {
            if (RepeatedEvent) return "\t.byte\t\t        " + Volume + Environment.NewLine;
            return "\t.byte\t\tVOL   , " + Volume + Environment.NewLine;
        }

        public string ToString(string mvlSymbolName)
        {
            if (RepeatedEvent) return "\t.byte\t\t        " + Volume + "*" + mvlSymbolName + "/mxv";
            return "\t.byte\t\tVOL   , " + Volume + "*" + mvlSymbolName + "/mxv";
        }
    }

    class PanSongEvent : SongEvent
    {
        public sbyte Pan { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public PanSongEvent(int eventOffset, sbyte pan, bool repeatedEvent) : base(eventOffset)
        {
            if (pan > 63 || pan < -64) throw new ArgumentException("pan out of -64 <--> +63 range at PanSongEvent()");
            Pan = pan;
            RepeatedEvent = repeatedEvent;
        }

        public override string ToString()
        {
            if (RepeatedEvent) return "\t.byte\t\t        c_v" + Pan.ToString("+00;-00;+00") + Environment.NewLine;
            return "\t.byte\t\tPAN   , c_v" + Pan.ToString("+00;-00;+00") + Environment.NewLine;
        }
    }

    class BendSongEvent : SongEvent
    {
        public sbyte Bend { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public BendSongEvent(int eventOffset, sbyte bend, bool repeatedEvent) : base(eventOffset)
        {
            if (bend > 63 || bend < -64) throw new ArgumentException("bend out of -64 <--> +63 range at BendSongEvent()");
            Bend = bend;
            RepeatedEvent = repeatedEvent;
        }

        public override string ToString()
        {
            if (RepeatedEvent) return "\t.byte\t\t        c_v" + Bend.ToString("+00,-00;+00") + Environment.NewLine;
            return "\t.byte\t\tBEND  , c_v" + Bend.ToString("+00;-00;+00") + Environment.NewLine;
        }
    }

    class BendrSongEvent : SongEvent
    {
        public byte BendR { get; private set; }

        public BendrSongEvent(int eventOffset, byte bendR) : base(eventOffset)
        {
            if (bendR > 127) throw new ArgumentException("bendr > 127 at BendrSongEvent()");
            BendR = bendR;
        }

        public override string ToString()
        {
            return "\t.byte\t\tBENDR , " + BendR + Environment.NewLine;
        }
    }

    class LfosSongEvent : SongEvent
    {
        public byte LfoS { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public LfosSongEvent(int eventOffset, byte lfoS) : base(eventOffset)
        {
            if (lfoS > 127) throw new ArgumentException("lfoS > 127 at LfosSongEvent()");
            LfoS = lfoS;
        }

        public override string ToString()
        {
            if (RepeatedEvent) return "\t.byte\t\t        " + LfoS + Environment.NewLine;
            return "\t.byte\t\tLFOS  , " + LfoS + Environment.NewLine;
        }
    }

    class LfodlSongEvent : SongEvent
    {
        public byte LfodDl { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public LfodlSongEvent(int eventOffset, byte lfoDl, bool repeatedEvent) : base(eventOffset)
        {
            if (lfoDl > 127) throw new ArgumentException("lfoDl > 127 at LfodlSongEvent()");
            LfodDl = lfoDl;
            RepeatedEvent = repeatedEvent;
        }

        public override string ToString()
        {
            if (RepeatedEvent) return "\t.byte\t\t        " + LfodDl + Environment.NewLine;
            return "\t.byte\t\tLFODL , " + LfodDl + Environment.NewLine;
        }
    }

    class ModSongEvent : SongEvent
    {
        public byte ModDepth { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public ModSongEvent(int eventOffset, byte modDepth, bool repeatedEvent) : base(eventOffset)
        {
            if (modDepth > 127) throw new ArgumentException("lfoDepth > 127 at ModSongEvent()");
            ModDepth = modDepth;
            RepeatedEvent = repeatedEvent;
        }

        public override string ToString()
        {
            if (RepeatedEvent) return "\t.byte\t\t        " + ModDepth + Environment.NewLine;
            return "\t.byte\t\tMOD   , " + ModDepth + Environment.NewLine;
        }
    }

    class ModtSongEvent : SongEvent
    {
        public byte ModT { get; private set; }

        public ModtSongEvent(int eventOffset, byte modT) : base(eventOffset)
        {
            if (modT > 2) throw new ArgumentException("modT > 2 at ModtSongEvent()");
            ModT = modT;
        }

        public override string ToString()
        {
            return "\t.byte\t\tMODT  , " + Tables.Mod[ModT] + Environment.NewLine;
        }
    }

    class TuneSongEvent : SongEvent
    {
        public sbyte Tune { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public TuneSongEvent(int eventOffset, sbyte tune, bool repeatedEvent) : base(eventOffset)
        {
            if (tune < -64 || tune > 63) throw new ArgumentException("tune out of valid range at TuneSongEvent()");
            Tune = tune;
            RepeatedEvent = repeatedEvent;
        }

        public override string ToString()
        {
            if (RepeatedEvent) return "\t.byte\t\t        c_v" + Tune.ToString("+00,-00;+00") + Environment.NewLine;
            return "\t.byte\t\tTUNE  , c_v" + Tune.ToString("+00,-00;+00") + Environment.NewLine;
        }
    }

    class XcmdSongEvent : SongEvent
    {
        public byte XcmdType { get; private set; }
        public byte XcmdPar { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public XcmdSongEvent(int eventOffset, byte xcmdType, byte xcmdPar, bool repeatedEvent) : base(eventOffset)
        {
            if (xcmdType > 127) throw new ArgumentException("xcmdType > 127 at XcmdSongEvent()");
            XcmdType = xcmdType;
            XcmdPar = xcmdPar;
            RepeatedEvent = repeatedEvent;
        }

        public override string ToString()
        {
            string type;
            switch (XcmdType)
            {
                case 0x8:
                    type = "xIECV";
                    break;
                case 0x9:
                    type = "xIECL";
                    break;
                default:
                    type = XcmdType.ToString();
                    break;
            }
            if (RepeatedEvent) return "\t.byte\t\t        " + type + " , " + XcmdPar + Environment.NewLine;
            return "\t.byte\t\tXCMD  , " + type + " , " + XcmdPar + Environment.NewLine;
        }
    }

    class NoteSongEvent : SongEvent
    {
        public byte NoteLength { get; private set; }
        public byte NoteVelocity { get; private set; }
        public byte NoteGateTime { get; private set; }
        public bool RepeatedEvent { get; private set; }

        public NoteSongEvent(int eventOffset, byte noteLength, byte noteVelocity, byte noteGateTime, bool repeatedEvent)
            : base(eventOffset)
        {
            
        }
    }
}
