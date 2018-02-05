using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            // Exeの場所を表示
            Console.WriteLine($"Path: {assembly.Location}");
            // バージョン番号を表示
            Console.WriteLine($"Version: {assembly.GetName().Version.ToString()}");

            Console.ReadKey();
        }
    }
}
