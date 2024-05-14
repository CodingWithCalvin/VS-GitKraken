using System;
using System.IO;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace CodingWithCalvin.GitKraken.Helpers
{
    internal static class ProjectHelpers
    {
        public static string GetSelectedPath(DTE2 dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            foreach (
                UIHierarchyItem selectedItem in (Array)
                    dte.ToolWindows.SolutionExplorer.SelectedItems
            )
            {
                switch (selectedItem.Object)
                {
                    case Solution solution:
                        return Directory.GetParent(solution.FileName).FullName;
                }
            }

            return null;
        }
    }
}
