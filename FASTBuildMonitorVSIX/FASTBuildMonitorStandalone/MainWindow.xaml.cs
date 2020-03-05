using System;
using System.Windows;
using System.Windows.Input;

namespace FASTBuildMonitorStandalone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Window = null;
        public MainWindow()
        {
            Window = this;
            InitializeComponent();

            this.Content = new FASTBuildMonitor.FASTBuildMonitorControl(
                "FASTBuildMonitor Standalone",
                version: "?", // TODO: retrieve those from AssemblyInfo
                authors: "?"
            );

            this.StateChanged += Window_StateChanged;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }
    }

    /// <summary>
    /// This simple class is used to handle click and double click on the tray icon(this restore and minimize the app window)
    /// </summary>
    public class ClickTrayIcon : ICommand
    {
        public void Execute(object parameter)
        {
            if (MainWindow.Window.WindowState == WindowState.Minimized)
                MainWindow.Window.WindowState = WindowState.Normal;
            else
                MainWindow.Window.WindowState = WindowState.Minimized;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
    }

}
