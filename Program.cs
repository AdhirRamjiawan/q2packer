using System;
using System.IO;

namespace Q2Packer
{
    class Program
    {
        const string VERSION = "1.0.1";

        static void Main(string[] args)
        {
            try
            {
                foreach (string arg in args)
                {
                    if (arg.ToLowerInvariant() == "--help")
                    {
                        PrintUsage();
                        return;
                    }
                }

                if (args.Length != 3)
                {
                    PrintUsage();
                    return;
                }

                if (args[0].ToLowerInvariant() == "pack")
                {
                    if (!Directory.Exists(args[1]))
                    {
                        Console.WriteLine("Error: Path to pack not found");
                        return;
                    }

                    using (PakPacker packer = new PakPacker(args[2]))
                    {
                        packer.Pack(args[1]);
                    }
                }
                else if (args[0].ToLowerInvariant() == "unpack")
                {
                    if (!File.Exists(args[1]))
                    {
                        Console.WriteLine("Error: Path to pak file not found");
                        return;
                    }

                    if (!Directory.Exists(args[2]))
                    {
                        Console.WriteLine("Error: cannot extract to path that does not exist");
                        return;
                    }

                    using (PakExtractor extractor = new PakExtractor(args[1]))
                    {
                        extractor.Extract(args[2]);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine($"\nQuake 2 Packer utility. Version {VERSION}");
            Console.WriteLine("Usage: ");
            Console.WriteLine("\tq2packer unpack <path to pak file> <output directory> ");
            Console.WriteLine("\tq2packer pack <input directory> <path to output pak file>");
            Console.WriteLine("General Options:");
            Console.WriteLine("\t--help print this instructions");
        }
    }
}