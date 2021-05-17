using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q2Packer
{
    public class PakHeader
    {
        public const int FileHeader = ('K' << 24) + ('C' << 16) + ('A' << 8) + 'P';
        public const int PakHeaderSize = 12;

        public int Header;
        public int LumpCollectionOffset;
        public int LumpCollectionSize;

        public static PakHeader ReadHeader(BinaryReader reader)
        {
            return new PakHeader()
            { 
                Header = reader.ReadInt32(),
                LumpCollectionOffset = reader.ReadInt32(),
                LumpCollectionSize = reader.ReadInt32()
            };
        }

        public static void WriteHeader(BinaryWriter writer, int fileTableOffset, int fileTableSize)
        {
            writer.Write(FileHeader);
            writer.Write(fileTableOffset);
            writer.Write(fileTableSize);
        }
    }
}
