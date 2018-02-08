using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

using Prism.Mvvm;
using Prism.Commands;

using Squirrel;

namespace SquirrelWindowsTest
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
        public DelegateCommand CheckForUpdateFromLocalButton { get; set; }
        public DelegateCommand UpdateFromLocalButton { get; set; }
        public DelegateCommand CheckForUpdateFromGitHubButton { get; set; }
        public DelegateCommand UpdateFromGitHubButton { get; set; }

        public MainWindowsViewModel()
        {
            var assembly = Assembly.GetExecutingAssembly();

            //Exeの場所を表示
            Path = assembly.Location;

            var fvi = FileVersionInfo.GetVersionInfo(Path);

            var assemblyVersion = assembly.GetName().Version.ToString();
            var fileVersion = fvi.FileVersion;
            var productVersion = fvi.ProductVersion;


            Version = $"{assembly.GetName().Name}" + Environment.NewLine
                + $"AssemblyVersion: {assemblyVersion}" + Environment.NewLine
                + $"AssemblyFileVersion: {fileVersion}" + Environment.NewLine
                + $"AssemblyInformationalVersion: {productVersion}"+ Environment.NewLine;



            CheckForUpdateFromLocalButton = new DelegateCommand(() => CheckForUpdate());
            UpdateFromLocalButton = new DelegateCommand(() => UpdateFromLocal());
            CheckForUpdateFromGitHubButton = new DelegateCommand(() => CheckForUpdateFromGitHub());
            UpdateFromGitHubButton = new DelegateCommand(() => UpdateFromGitHub());
        }

        async void CheckForUpdate()
        {
            string str = "ローカルチェック" + Environment.NewLine;

            try
            {
                using (var mgr = new UpdateManager(@"C:\Users\13005\git\github\SquirrelWindowsTest\Releases"))
                {
                    var updateinfo = await mgr.CheckForUpdate();

                    SetUpdateInfo(ref str, updateinfo);
                    str += "アップデート" + (UpdateExists(updateinfo) ? "あり" : "なし") + Environment.NewLine;
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

        async void UpdateFromLocal()
        {
            string str = "ローカルからアップデート" + Environment.NewLine;

            try
            {
                using (var mgr = new UpdateManager(@"C:\Users\13005\git\github\SquirrelWindowsTest\Releases"))
                {
                    var updateinfo = await mgr.CheckForUpdate();

                    if (UpdateExists(updateinfo))
                    {
                        var releaseEntry = await mgr.UpdateApp();
                        str += $"{releaseEntry.Version} へアップデート開始" + Environment.NewLine;
                        str += "完了" + Environment.NewLine;
                    }
                    else
                    {
                        str += "アップデートなし" + Environment.NewLine;
                    }
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
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/kuttsun/SquirrelWindowsTest"))
                {
                    var updateinfo = await mgr.CheckForUpdate();

                    SetUpdateInfo(ref str, updateinfo);
                    str += "アップデート" + (UpdateExists(updateinfo) ? "あり" : "なし") + Environment.NewLine;
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

        async void UpdateFromGitHub()
        {
            string str = "GitHub からアップデート" + Environment.NewLine;

            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/kuttsun/SquirrelWindowsTest"))
                {
                    var releaseEntry = await mgr.UpdateApp();

                    str += $"{releaseEntry.Version} へアップデート開始";
                    str += "完了" + Environment.NewLine;
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

        bool UpdateExists(UpdateInfo updateinfo)
        {
            try
            {
                if (updateinfo.CurrentlyInstalledVersion?.Version < updateinfo.FutureReleaseEntry?.Version)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
