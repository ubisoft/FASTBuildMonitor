//------------------------------------------------------------------------------
// Copyright 2017 Yassine Riahi and Liam Flookes. 
// Provided under a MIT License, see license file on github.
//------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace FASTBuildMonitorVSIX
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("a73d6380-6d5f-4cab-8490-72ca61772d09")]
    public class FASTBuildMonitorPane : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FASTBuildMonitor"/> class.
        /// </summary>
        public FASTBuildMonitorPane() : base(null)
        {
            this.Caption = "FASTBuild Monitor";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            var vsixPackageInfo = FASTBuildMonitorPackage.GetCurrentVSIXPackageInformation();
            var monitorControl = new FASTBuildMonitor.FASTBuildMonitorControl(
                vsixPackageInfo._packageName,
                vsixPackageInfo._version,
                vsixPackageInfo._authors
            );

            monitorControl.OnPreviewDocumentClick += MonitorControl_OnPreviewDocumentClick;

            this.Content = monitorControl;
        }

        private void MonitorControl_OnPreviewDocumentClick(string filePath, int lineNumber)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            ThreadHelper.ThrowIfNotOnUIThread();
            VsShellUtilities.OpenDocument(FASTBuildMonitorPackage._instance, filePath);

            DTE2 _dte = FASTBuildMonitorPackage._instance._dte;

            //Console.WriteLine("Window: {0}", _dte.ActiveWindow.Caption);

            TextSelection sel = _dte.ActiveDocument.Selection as EnvDTE.TextSelection;

            sel.StartOfDocument(false);
            sel.EndOfDocument(true);
            sel.GotoLine(lineNumber);

            try
            {
                sel.ActivePoint.TryToShow(vsPaneShowHow.vsPaneShowCentered, null);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Exception! " + ex.ToString());
            }
        }
    }
}
