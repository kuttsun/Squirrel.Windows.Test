using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Windows;

namespace Squirrel.Windows.Test
{
    class MainWindowsViewModel
    {
        public string Path { get; set; }
        public string Version { get; set; }

        public MainWindowsViewModel()
        {
            var assembly = Assembly.GetExecutingAssembly();
            //Exeの場所を表示
            Path = assembly.Location;
            //バージョン番号を表示
            Version = assembly.GetName().Version.ToString();
        }
    }
}
