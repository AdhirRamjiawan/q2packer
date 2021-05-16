using System;
using System.IO;

namespace Q2Packer
{
    class Program
    {
        const string VERSION = "1.0.0";

        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
                    PrintUsage();
                    return;
                }

                if (!File.Exists(args[0]))
                {
                    Console.WriteLine("Error: Path to pak file not found");
                    return;
                }

                if (!Directory.Exists(args[1]))
                {
                    Console.WriteLine("Error: cannot extract to path that does not exist");
                    return;
                }

                foreach (string arg in args)
                {
                    if (arg.ToLowerInvariant() == "--help")
                    {
                        PrintUsage();
                        return;
                    }
                }

                using (PakExtractor extractor = new PakExtractor(args[0]))
                {
                    extractor.Extract(args[1]);
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
            Console.WriteLine("usage: ");
            Console.WriteLine("\tq2packer <path to pak file> <output directory> ");
            Console.WriteLine("options:");
            Console.WriteLine("\t--help print this instructions");
        }
    }
}