using System;
using System.ComponentModel;
using System.IO;
using Microsoft.VisualStudio.Shell;

namespace CodingWithCalvin.VSKraken.Dialogs
{
    public class SettingsDialogPage : DialogPage
	{
		[Category("General")]
		[DisplayName("GitKraken Installation Path")]
		[Description("The absolute path to GitKraken's \"Update.exe\" file, including the executable.")]
		public string UpdateExecutablePath { get; set; }

		public override void LoadSettingsFromStorage()
		{
			base.LoadSettingsFromStorage();

			if (!string.IsNullOrEmpty(this.UpdateExecutablePath))
			{
				return;
			}

			this.UpdateExecutablePath = FindGitKrakenInstallDirectory();
		}

		private static string FindGitKrakenInstallDirectory()
		{
			var directoryInfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

			foreach (var directory in directoryInfo.GetDirectories("gitkraken"))
			{
				var path = Path.Combine(directory.FullName, "Update.exe");
				if (File.Exists(path))
				{
					return path;
				}
			}

			return null;
		}
	}
}
