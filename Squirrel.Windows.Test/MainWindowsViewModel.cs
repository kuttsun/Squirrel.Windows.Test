using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Windows;

using Prism.Mvvm;
using Prism.Commands;

using Squirrel;

namespace Squirrel.Windows.Test
{
    class MainWindowsViewModel : BindableBase
    {
        public string Path { get; set; }
        public string Version { get; set; }
        public DelegateCommand Button { get; set; }

        public MainWindowsViewModel()
        {
            var assembly = Assembly.GetExecutingAssembly();
            //Exeの場所を表示
            Path = assembly.Location;
            //バージョン番号を表示
            Version = assembly.GetName().Version.ToString();

            Button = new DelegateCommand(async () =>
            {
                using (var mgr = new UpdateManager("C:\\Projects\\MyApp\\Releases"))
                {
                    var ret = await mgr.CheckForUpdate();
                }
            });
        }
    }
}
