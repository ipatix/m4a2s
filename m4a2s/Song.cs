using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m4a2s
{
    class Song
    {
        private int offset;

        private byte numTracks;
        private byte numBlocks;
        private byte priority;
        private byte reverb;
        private int voicegroupPtr;
        private int[] tracksPtr;

        private Stream inputStream;

        public Song(Stream inputStream, int offset)
        {
            this.inputStream = inputStream;
            this.offset = offset;
        }
    }

    struct SongNode
    {
        public Song Song;
        public byte MusicPlayer;
    }

    class Track
    {
        
    }
}
