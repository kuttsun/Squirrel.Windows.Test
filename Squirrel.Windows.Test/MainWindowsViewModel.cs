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
                await Task.Run(() => CheckForUpdate());
            });
        }

        void CheckForUpdate()
        {
            //using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/kuttsun/Squirrel.Windows.Test/releases/latest"))
            using (var mgr = new UpdateManager(@"C:\Users\13005\git\github\Squirrel.Windows.Test\Releases"))
            {
                try
                {
                    var updateInfo = mgr.CheckForUpdate().Result;

                    Console.WriteLine($"Current Version: {updateInfo.CurrentlyInstalledVersion}");
                    Console.WriteLine($"Latest Version: {updateInfo.FutureReleaseEntry}");

                    foreach (var entry in updateInfo.ReleasesToApply)
                    {
                        Console.WriteLine($"- Filename : {entry.Filename}");
                    }
                }
                catch
                {
                    Console.WriteLine("No releases.");
                }
            }
        }
    }
}
