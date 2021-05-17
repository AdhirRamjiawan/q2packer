using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Q2Packer
{
    public class PakPacker : IDisposable
    {
        private BinaryWriter fileWriter;
        private List<PakLump> data;
        private int runningOffset;
        private string inputDirectory;

        public PakPacker(string outputFile)
        {
            fileWriter = new BinaryWriter(new FileStream(outputFile, FileMode.CreateNew));
            data = new List<PakLump>();
        }

        public void Pack(string inputDirectory)
        {
            this.inputDirectory = inputDirectory;
            BuildLumpCollection(inputDirectory);
            ProcessLumpOffsets();

            PakHeader.WriteHeader(fileWriter, PakHeader.PakHeaderSize, data.Count * 64);
            PakFileTable.WriteFileTable(fileWriter, data);

            foreach (var lump in data)
            {
                fileWriter.Write(lump.Data);
            }
        }
        
        public void Dispose()
        {
            fileWriter.Close();
            fileWriter.Dispose();
        }

        private void ProcessLumpOffsets()
        {
            runningOffset = PakHeader.PakHeaderSize + (data.Count * 64);

            foreach (var lump in data)
            {
                lump.Offset = runningOffset;
                runningOffset += lump.Size;
            }
        }

        private void BuildLumpCollection(string path)
        {
            var entries = Directory.GetFileSystemEntries(path);

            foreach (string entry in entries)
            {
                try
                {
                    if (File.GetAttributes(entry).HasFlag(FileAttributes.Directory))
                    {
                        BuildLumpCollection(entry);
                    }
                    else
                    {
                        string lumpName = entry.Replace(inputDirectory, "");

                        if (lumpName.Length > 55)
                            throw new Exception("Lump name is too long. Halting...");

                        byte[] bytes = File.ReadAllBytes(entry);
                        data.Add(new PakLump() { Name = lumpName, Data = bytes, Size = bytes.Length });
                    }
                }
                catch
                {
                    Console.WriteLine($"Failed at checking sub-directory length for path {entry}");
                    throw;
                }
            }
        }
    }
}
