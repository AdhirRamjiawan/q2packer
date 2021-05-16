using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q2Packer
{
    public class Debugging
    {
        public static bool isDebug = false;

        public static void debug(string message, params object[] args)
        {
            Console.WriteLine(message);
        }
    }
}
