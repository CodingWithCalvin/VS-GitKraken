using System;
using System.Runtime.InteropServices;
using System.Threading;
using CodingWithCalvin.GitKraken.Commands;
using CodingWithCalvin.GitKraken.Dialogs;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace CodingWithCalvin.GitKraken
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuids.guidPackageString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(
        typeof(SettingsDialogPage),
        Vsix.Name,
        "General",
        101,
        111,
        true,
        new string[0],
        ProvidesLocalizedCategoryName = false
    )]
    public sealed class GitKrakenPackage : AsyncPackage
    {
        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress
        )
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var settings = (SettingsDialogPage)this.GetDialogPage(typeof(SettingsDialogPage));

            OpenCommand.Initialize(this, settings);
        }
    }
}
