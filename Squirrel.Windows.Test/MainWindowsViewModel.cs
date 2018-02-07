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
        public string Path
        {
            get { return path; }
            set { SetProperty(ref path, value); }
        }
        string path;

        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }
        string version;
        public string Result
        {
            get { return result; }
            set { SetProperty(ref result, value); }
        }
        string result;
        public DelegateCommand UpdateCheckLocalButton { get; set; }
        public DelegateCommand UpdateCheckGitHubButton { get; set; }

        public MainWindowsViewModel()
        {
            var assembly = Assembly.GetExecutingAssembly();
            //Exeの場所を表示
            Path = assembly.Location;
            //バージョン番号を表示
            Version = assembly.GetName().Version.ToString();

            UpdateCheckLocalButton = new DelegateCommand(() => CheckForUpdate());

            UpdateCheckGitHubButton = new DelegateCommand(() => CheckForUpdateFromGitHub());
        }

        async void CheckForUpdate()
        {
            string str = "ローカルチェック" + Environment.NewLine;

            try
            {
                using (var mgr = new UpdateManager(@"C:\Users\13005\git\github\Squirrel.Windows.Test\Releases"))
                {
                    var updateinfo = await mgr.CheckForUpdate();

                    SetUpdateInfo(ref str, updateinfo);
                }
            }
            catch (Exception e)
            {
                str += e.Message + Environment.NewLine;
                if (e.InnerException != null)
                {
                    str += e.InnerException + Environment.NewLine;
                }
            }

            Result = str;
        }

        async void CheckForUpdateFromGitHub()
        {
            string str = "GitHub チェック" + Environment.NewLine;

            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/kuttsun/Squirrel.Windows.Test"))
                {
                    var updateinfo = await mgr.CheckForUpdate();

                    SetUpdateInfo(ref str, updateinfo);
                }
            }
            catch (Exception e)
            {
                str += e.Message + Environment.NewLine;
                if (e.InnerException != null)
                {
                    str += e.InnerException + Environment.NewLine;
                }
            }

            Result = str;
        }

        void SetUpdateInfo(ref string str, UpdateInfo updateinfo)
        {
            str += $"Current Version: {updateinfo.CurrentlyInstalledVersion?.Version}" + Environment.NewLine;
            str += $"Latest Version: {updateinfo.FutureReleaseEntry?.Version}" + Environment.NewLine;

            foreach (var entry in updateinfo.ReleasesToApply)
            {
                str += $"- Filename : {entry.Filename}" + Environment.NewLine;
                str += $"- Version : {entry.Version}" + Environment.NewLine;
            }
        }
    }
}
