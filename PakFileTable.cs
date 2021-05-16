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
            byte[] nameBytes = Encoding.UTF8.GetBytes(lumpName);
            writer.Write(nameBytes);
            writer.Write(lumpOffset);
            writer.Write(lumpSize);

            // pad to 64 bytes
            for (int i = nameBytes.Length; i <= 64; i++)
            {
                writer.Write(0);
            }
        }
    }
}
