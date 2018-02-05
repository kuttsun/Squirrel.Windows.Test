using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using Squirrel;

namespace ConsoleApp
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

            //using (var mgr = new Updateanager(@"C:\projects\myapp\releases"))
            //{
            //    var ret = await mgr.checkforupdate();
            //}

            Console.ReadKey();
        }
    }
}
