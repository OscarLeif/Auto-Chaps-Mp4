using System.Windows;
using System;
using System.Diagnostics;
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
        //The folder to fix the mp4 files
        public String FolderPath { get; set; }

        public String ActualFileName { get; set; }

        //The location of the mp4Chaps.exe
        public String mp4Chaps;

        public String commandString { get; set; }
        public String argumentChap { get; set; }

        //This can handle the external .exe files
        private Process Process;

        public MainWindow()
        {
            InitializeComponent();
            LoadMp4Chaps();
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

        public void LoadMp4Chaps()
        {
            //This should be loaded in the bin folder
            mp4Chaps = "mp4chaps.exe";
            //System.Diagnostics.Process.Start(mp4Chaps);
            File.Exists(mp4Chaps);
            Debug.Write("Mp4 Chaps exist in the project");
        }


        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                MessageBox.Show("You selected: " + dialog.FileName);
                FolderPath = dialog.FileName;
                textBoxPath.Text = FolderPath;
            }
        }

        private void createChapters()
        {           
            //First get all the mp4 file from the 
            foreach (string file in Directory.GetFiles(FolderPath, "*mp4"))
            {
                Process = new Process();
                //Begining The Mp4 Chaps Exe
                Process.StartInfo.FileName = mp4Chaps;
                // Add the arguments create every 600 seconds a chapter in the mp4 files
                Process.StartInfo.Arguments = "-e 600 \"" + file + "\"";
                //Start the Exe mp4Chaps
                Process.Start();
                //But we start the mp4chaps only one by one this will
                //avoid starting multiple instances of the mp4chaps
                Process.WaitForExit();

                ///Rename file process
                FileInfo fileInfo = new FileInfo(file);
                string name = fileInfo.Name;
                if (name.Contains("XXX"))
                {
                    string str = name.Replace('.', ' ');
                    int length = str.LastIndexOf("XXX");
                    string newName = str.Substring(0, length) + "XXX.mp4";
                    fileInfo.Rename(newName);
                }

                UpdateMetadaName(fileInfo.FullName);
            }           
        }

        //This requieres TagLib (AKA TagSharp Lib)
        public void UpdateMetadaName(String PathFile)
        {
            FileInfo fileINfo = new FileInfo(PathFile);

            var file = TagLib.File.Create(PathFile);
            file.Tag.Title = Path.GetFileNameWithoutExtension(fileINfo.Name);
            file.Save();
        }

        public void FixFileName(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            string name = fileInfo.Name;
            if (name.Contains("XXX"))
            {
                string str = name.Replace('.', ' ');
                int length = str.IndexOf("XXX");
                string newName = str.Substring(0, length) + "XXX.mp4";
                fileInfo.Rename(newName);

                ActualFileName = fileInfo.Name;
                //System.IO.File.Move(name, newName);
                //File.Move("sourcename.ext", "newname.ext");
            }
        }

        public void Rename(FileInfo fileInfo, string newName)
        {
            fileInfo.MoveTo(fileInfo.Directory.FullName + "\\" + newName);
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            //Check if the directory path is ready

            //Check if mp4Chaps exist

            //Start creating the job

            createChapters();


        }
    }


}
