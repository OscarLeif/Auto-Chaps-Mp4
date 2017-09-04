using System.Windows;
using System;
using System.Drawing;
using System.IO;
using Microsoft.Win32;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace AutoChapsMp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSetup_Click(object sender, RoutedEventArgs e)
        {
            bool isWindowOpen = false;

            //Avoid Creating the same window setting
            foreach (Window w in Application.Current.Windows)
            {
                if (w is SettingsWindow)
                {
                    isWindowOpen = true;
                    w.Activate();
                }
            }

            if (!isWindowOpen)
            {
                SettingsWindow win = new SettingsWindow();
                win.Show();
            }
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                MessageBox.Show("You selected: " + dialog.FileName);
                textBoxPath.Text = dialog.FileName;
            }
        }
    }
}
