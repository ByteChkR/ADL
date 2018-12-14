using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            while (Console.OpenStandardInput().CanRead)
            {
                string test = Console.ReadLine();
                if (test != null && test != "") Console.WriteLine(test);
            }
        }
    }
}
