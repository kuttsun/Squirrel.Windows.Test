using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Squirrel;

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

            Console.WriteLine("-----");

            // アップデートがあるかどうかをチェック
            CheckForUpdate();
            //CheckForUpdateFromGitHub();

            Console.ReadKey();
        }

        static void CheckForUpdate()
        {
            Console.WriteLine("ローカルチェック");

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

        static void CheckForUpdateFromGitHub()
        {
            Console.WriteLine("GitHub チェック");

            using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/kuttsun/Squirrel.Windows.Test/releases/latest"))
            {
                try
                {
                    var updateInfo = mgr.Result.CheckForUpdate().Result;

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
