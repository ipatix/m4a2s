using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m4a2s
{
    class Voicegroup
    {
        public int VoicegroupOffset { get; private set; }
        public ushort NumInstruments { get; private set; }
        public VoicegroupEntry[] Entries { get; private set; }

        public Voicegroup(int voicegroupOffset, ushort numInstruments, VoicegroupEntry[] entries)
        {
            VoicegroupOffset = voicegroupOffset;
            NumInstruments = numInstruments;
            Entries = entries;
        }
    }

    abstract class VoicegroupEntry
    {
        public byte InstrType { get; private set; }
        public byte InstrKey { get; private set; }
        // public byte InstrType { get { return _instrType; } }
        // public byte InstrKey { get { return _instrKey; } }

        protected VoicegroupEntry(byte instrType, byte instrKey)
        {
            InstrType = instrType;
            InstrKey = instrKey;
        }

        public abstract override string ToString();
    }

    class WaveVgrEntry : VoicegroupEntry
    {
        public Wave SamplePointer { get; private set; }
        public uint Adsr { get; private set; }

        public WaveVgrEntry(byte instrType, byte instrKey, Wave samplePointer, uint adsr) : base(instrType, instrKey)
        {
            SamplePointer = samplePointer;
            Adsr = adsr;
        }
    }

    class FixedWaveVgrEntry : VoicegroupEntry
    {
        public Wave SamplePointer { get; private set; }
        public uint Adsr { get; private set; }
        public byte Pan { get; private set; }

        public FixedWaveVgrEntry(byte instrType, byte instrKey, byte pan, Wave samplePointer, uint adsr) : base(instrType, instrKey)
        {
            SamplePointer = samplePointer;
            Adsr = adsr;
            Pan = pan;
        }
    }

    class PsgVgrEntry : VoicegroupEntry
    {
        public byte Length { get; private set; }
        public byte Sweep { get; private set; }
        public uint Pattern { get; private set; }
        public uint Adsr { get; private set; }

        public PsgVgrEntry(byte instrType, byte instrKey, byte length, byte sweep, uint pattern, uint adsr) : base (instrType, instrKey)
        {
            Length = length;
            Sweep = sweep;
            Pattern = pattern;
            Adsr = adsr;
        }
    }

    class WavePsgVgrEntry : VoicegroupEntry
    {
        public byte Length { get; private set; }
        public byte Sweep { get; private set; }
        public PsgWave WavePtr { get; private set; }
        public uint Adsr { get; private set; }

        public WavePsgVgrEntry(byte instrType, byte instrKey, byte length, byte sweep, PsgWave wavePtr, uint adsr) : base (instrType, instrKey)
        {
            Length = length;
            Sweep = sweep;
            WavePtr = wavePtr;
            Adsr = adsr;
        }
    }

    class DrumTableVgrEntry : VoicegroupEntry
    {
        public Voicegroup DrumTablePtr { get; private set; }

        public DrumTableVgrEntry(byte instrType, Voicegroup drumTablePtr) : base(instrType, 0x0)
        {
            DrumTablePtr = drumTablePtr;
        }
    }

    class MultiPartVgrEntry : VoicegroupEntry
    {
        public Voicegroup SubVoicegroup { get; private set; }
        public KeyMap InstrKeyMap { get; private set; }

        public MultiPartVgrEntry(byte instrType, Voicegroup subVoicegroup, KeyMap instrKeyMap) : base(instrType, 0x0)
        {
            SubVoicegroup = subVoicegroup;
            InstrKeyMap = instrKeyMap;
        }
    }

    class KeyMap
    {
        private readonly byte[] _instrMap;
        public int KeyMapOffset { get; private set; }

        public byte InstrMap(byte midiKey)
        {
            if (midiKey > 127) throw new IndexOutOfRangeException("Midi Key out of range at InstrMap() call");
            return _instrMap[midiKey];
        }

        public KeyMap(byte[] instrMap, int keyMapOffset)
        {
            _instrMap = instrMap;
            KeyMapOffset = keyMapOffset;
        }
    }
}
