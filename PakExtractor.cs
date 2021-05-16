using System;
using System.IO;
using System.Text;

namespace Q2Packer
{
    public class PakExtractor : IDisposable
    {
        private BinaryReader fileReader;

        private const int pakHeader = ('K'  << 24) + ('C' << 16) + ('A' << 8) + 'P';
        //private string colourMapLumpName = "pics/colormap.pcx";

        public PakExtractor(string filePath)
        {
            fileReader = new BinaryReader(new FileStream(filePath, FileMode.Open));
        }

        public void Extract(string outputPath)
        {
            var id = fileReader.ReadInt32();

            if (id != pakHeader)
                throw new Exception("Invalid Pak file! Could not match pak header");

            var lumpCollectionOffset = fileReader.ReadInt32();
            var lumpCollectionLength = fileReader.ReadInt32();

            // move the file stream passed header(dirOfs -12 bytes)
            fileReader.BaseStream.Position += (lumpCollectionOffset - 12);

            var lumpCount = lumpCollectionLength / 64;

            for (int lumpIndex = 0; lumpIndex < lumpCount; lumpIndex++)
            {
                var lumpName = ReadString(56);

                var lumpFilePosition = fileReader.ReadInt32();
                var lumpFileLength = fileReader.ReadInt32();

                var outputFilePath = $"{outputPath}\\{lumpName}";
                outputFilePath = outputFilePath.Replace("\"", "").Replace('/', '\\');

                Directory.CreateDirectory(outputFilePath.Substring(0, outputFilePath.LastIndexOf("\\")));

                long tempPosition = fileReader.BaseStream.Position;
                fileReader.BaseStream.Position = lumpFilePosition;
                    
                File.WriteAllBytes(outputFilePath, fileReader.ReadBytes(lumpFileLength));

                fileReader.BaseStream.Position = tempPosition;
            }
        }

        public void Dispose()
        {
            fileReader.Close();
            fileReader.Dispose();
        }

        private string ReadString(int length)
        {
            var data = Encoding.UTF8.GetString(fileReader.ReadBytes(length));
            data = data.Substring(0, data.IndexOf('\0'));
            return data;
        }
    }
}
