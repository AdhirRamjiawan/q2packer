using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q2Packer
{
    public class PakLumpLister : IDisposable
    {
        private BinaryReader fileReader;

        public PakLumpLister(string filePath)
        {
            fileReader = new BinaryReader(new FileStream(filePath, FileMode.Open));
        }

        public void List()
        {
            var id = fileReader.ReadInt32();

            if (id != PakHeader.FileHeader)
                throw new Exception("Invalid Pak file! Could not match pak header");

            var lumpCollectionOffset = fileReader.ReadInt32();
            var lumpCollectionLength = fileReader.ReadInt32();

            // move the file stream passed header(dirOfs -12 bytes)
            fileReader.BaseStream.Position += (lumpCollectionOffset - 12);

            var lumpCount = lumpCollectionLength / 64;

            for (int lumpIndex = 0; lumpIndex < lumpCount; lumpIndex++)
            {
                var lumpName = ReadString(56);
                fileReader.BaseStream.Position += 8;
                Console.WriteLine(lumpName);
            }
        }

        public void Dispose()
        {
            fileReader.Close();
            fileReader.Dispose();
        }

        private string ReadString(int length)
        {
            var data = Encoding.ASCII.GetString(fileReader.ReadBytes(length));
            data = data.Substring(0, data.IndexOf('\0'));
            return data;
        }
    }
}
