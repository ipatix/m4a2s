/*
 * m4a2s is aprogram to to dump sound files from m4a/mp2k GBA games to pseudo-assembly source files
 * Copyright (C) 2015 ipatix
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA
 */

namespace m4a2s
{
    class Entity
    {
        public EntityType Type { get; private set; }
        public int Offset { get; private set; }
        public string Guid { get; private set; }

        public int VoicegroupLength { get; private set; }


        public Entity(EntityType type, int offset, string guid, int voicegroupLength)
        {
            Type = type;
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
