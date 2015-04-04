namespace m4a2s
{
    class Entity
    {
        private EntityType _type;
        public int Offset { get; private set; }
        public string Guid { get; private set; }

        public int VoicegroupLength { get; private set; }


        public Entity(EntityType type, int offset, string guid, int voicegroupLength)
        {
            _type = type;
            VoicegroupLength = voicegroupLength;
            Offset = offset;
            Guid = guid;
        }
    }

    enum EntityType
    {
        Song, Bank, Wave, GbWave, KeyMap
    }
}
