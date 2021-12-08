using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using EnvDTE80;
using EnvDTE;
using System.Windows.Forms;
using CodingWithCalvin.VSKraken.Helpers;
using CodingWithCalvin.VSKraken.Dialogs;

namespace CodingWithCalvin.VSKraken.Commands
{
    internal class OpenCommand
    {
        private readonly Package _package;
        private readonly SettingsDialogPage _settings;

        public static OpenCommand Instance { get; private set; }
        private IServiceProvider ServiceProvider => this._package;

        private OpenCommand(Package package, SettingsDialogPage settings)
        {
            this._package = package;
            this._settings = settings;

            var commandService = (OleMenuCommandService)ServiceProvider.GetService(typeof(IMenuCommandService));

            if (commandService != null)
            {
                var menuCommandId = new CommandID(PackageGuids.guidOpenInGKCmdSet, PackageIds.OpenInGK);
                var menuItem = new MenuCommand(OpenPath, menuCommandId);
                commandService.AddCommand(menuItem);
            }
        }

        public static void Initialize(Package package, SettingsDialogPage settings)
        {
            Instance = new OpenCommand(package, settings);
        }

        private void OpenPath(object sender, EventArgs e)
        {
            var service = (DTE2)this.ServiceProvider.GetService(typeof(DTE));
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var gitRepository = ProjectHelpers.GetSelectedPath(service);
                var gitkrakenUpdateExecutablePath = _settings.UpdateExecutablePath;

                if (!string.IsNullOrEmpty(gitRepository) && !string.IsNullOrEmpty(gitkrakenUpdateExecutablePath))
                {
                    OpenExecutable(gitRepository, gitkrakenUpdateExecutablePath);
                }
                else
                {
                    MessageBox.Show("Unable to find a compatible git repository for the selected solution.");
                }
            }
            catch (Exception ex)
            {
                // Logger.Log(ex);
            }
        }

        private static void OpenExecutable(string gitRepository, string gitkrakenUpdateExecutablePath)
        {
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = gitRepository,
                FileName = $@"{gitkrakenUpdateExecutablePath}",
                Arguments = $@"--processStart=gitkraken.exe --process-start-args=""-p ""{ gitRepository }""",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };

            using (System.Diagnostics.Process.Start(startInfo)) { }
        }
    }
}
