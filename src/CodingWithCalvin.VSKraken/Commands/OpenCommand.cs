using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using EnvDTE80;
using EnvDTE;
using System.Windows.Forms;
using CodingWithCalvin.VSKraken.Helpers;

namespace CodingWithCalvin.VSKraken.Commands
{
    internal class OpenCommand
    {
        private readonly Package package;
        public static OpenCommand Instance { get; private set; }
        private IServiceProvider ServiceProvider => this.package;

        private OpenCommand(Package package)
        {
            this.package = package;

            var commandService = (OleMenuCommandService)ServiceProvider.GetService(typeof(IMenuCommandService));

            if (commandService != null)
            {
                var menuCommandId = new CommandID(PackageGuids.guidOpenInGKCmdSet, PackageIds.OpenInGK);
                var menuItem = new MenuCommand(OpenPath, menuCommandId);
                commandService.AddCommand(menuItem);
            }
        }

        public static void Initialize(Package package)
        {
            Instance = new OpenCommand(package);
        }

        private void OpenPath(object sender, EventArgs e)
        {
            var service = (DTE2)this.ServiceProvider.GetService(typeof(DTE));
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var selectedFilePath = ProjectHelpers.GetSelectedPath(service);
                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    OpenExecutable(selectedFilePath);
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

        private static void OpenExecutable(string gitRepository)
        {
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = gitRepository,
                FileName = "gitkraken",
                //Arguments = $"-p \"{gitRepository}\"",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (System.Diagnostics.Process.Start(startInfo))
            {
                //TODO : Should this be empty?
            }
        }
    }
}
