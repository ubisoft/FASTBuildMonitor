//------------------------------------------------------------------------------
// Copyright 2017 Yassine Riahi and Liam Flookes. 
// Provided under a MIT License, see license file on github.
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace FASTBuildMonitorVSIX
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class FASTBuildMonitorCommand
    {
        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="FASTBuildMonitorCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private FASTBuildMonitorCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID1 = new CommandID(PackageGuids.guidFASTBuildMonitorPackageCmdSet, PackageIds.FASTBuildMonitorCommandId);
                var menuItem1 = new MenuCommand(this.ShowToolWindow, menuCommandID1);
                commandService.AddCommand(menuItem1);

                var menuCommandID2 = new CommandID(PackageGuids.guidFASTBuildMonitorPackageCmdSet, PackageIds.FASTBuildMonitorToolsCommandId);
                var menuItem2 = new MenuCommand(this.ShowToolWindow, menuCommandID2);
                commandService.AddCommand(menuItem2);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static FASTBuildMonitorCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new FASTBuildMonitorCommand(package);
        }

        /// <summary>
        /// Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = this.package.FindToolWindow(typeof(FASTBuildMonitorPane), 0, true);
            if (window?.Frame == null)
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            ThreadHelper.ThrowIfNotOnUIThread();
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
