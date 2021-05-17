using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Q2Packer
{
    public class PakFileTable
    {
        public static void WriteFileTable(BinaryWriter writer, List<PakLump> data)
        {
            foreach (var lump in data)
            {
                WriteFileTableEntry(writer, lump.Name, lump.Offset, lump.Size);
            }
        }

        public static void WriteFileTableEntry(BinaryWriter writer, string lumpName, int lumpOffset, int lumpSize)
        {
            byte[] nameBytes = Encoding.ASCII.GetBytes(lumpName + '\0');
            writer.Write(nameBytes);

            for (int i = nameBytes.Length; i < 56; i++)
            {
                writer.Write('\0');
            }

            writer.Write(lumpOffset);
            writer.Write(lumpSize);
        }
    }
}
