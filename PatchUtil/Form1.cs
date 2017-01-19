using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PatchUtil
{
    public partial class Form1 : Form
    {
        private Patch patch = null;

        [System.Runtime.InteropServices.DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length != 2)
            {
                filesList.Items.Add("Open with patch file as an argument");
            }
            else if (!File.Exists(args[1]))
            {
                filesList.Items.Add("The file does not exist!");
            }
            else
            {
                string patchPath = args[1];
                patch = new Patch(patchPath);
                patch.GetFileList().ForEach((string filename) =>
                {
                    this.filesList.Items.Add(new FileListItem(filename));
                });
                if (filesList.Items.Count <= 0)
                {
                    filesList.Items.Add("No files to patch found!"); return;
                }
                filesList.SelectedIndex = 0;
                butPatchAll.Enabled = true;
            }
        }

        private string PatchFile(string fileName)
        {
            string tempFilePath = Path.GetTempFileName();

            if (patch.ApplyPatch(fileName, tempFilePath)) return tempFilePath;
            return null;
        }

        private void butPatchSelected_Click(object sender, EventArgs e)
        {
            var selectedIndices = filesList.SelectedIndices;
            for (int i = 0; i < selectedIndices.Count; i++)
            {
                int index = selectedIndices[i];
                var fileItem = filesList.Items[index] as FileListItem;
                if (fileItem != null)
                {
                    if (!ApplyPatch(index))
                    {
                        MessageBox.Show("Error", $"Failed to apply patch on file {fileItem.FileName}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private bool ApplyPatch(int index)
        {
            butPatchSelected.Enabled = true;
            string fileName = (filesList.Items[index] as FileListItem).FileName;
            string originalFilePath = Path.Combine(patch.PatchFileInfo.Directory.FullName, fileName.Replace('/', '\\'));
            string patchedFile = PatchFile(fileName);
            if (patchedFile == null)
            {
                MessageBox.Show($"Failed to patch file {fileName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            File.Delete(originalFilePath);
            File.Move(patchedFile, originalFilePath);
            return true;
        }

        private void butPatchAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < filesList.Items.Count; i++)
            {
                if (filesList.Items[i] is FileListItem)
                {
                    ApplyPatch(i);
                }
            }
            MessageBox.Show("Done", "All files processed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void filesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (filesList.SelectedIndex != -1)
            {
                butPatchSelected.Enabled = true;
                var fileItem = filesList.Items[filesList.SelectedIndex] as FileListItem;
                if (fileItem == null) return;
                string fileName = fileItem.FileName;
                Process p = fileItem.ProcessRef;
                if (p != null && !p.HasExited)
                {
                    var handle = p.MainWindowHandle;
                    if (handle == IntPtr.Zero) return;
                    SetForegroundWindow(handle);
                }
                else
                {
                    string originalFilePath = Path.Combine(patch.PatchFileInfo.Directory.FullName, fileName.Replace('/', '\\'));
                    string patchedFile = PatchFile(fileName);
                    if (patchedFile == null)
                    {
                        MessageBox.Show("Failed to patch file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    p = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = AppSettings.Instance.DiffToolPath,
                            Arguments = $"\"{originalFilePath}\" \"{patchedFile}\"",
                            CreateNoWindow = false
                        }
                    };
                    p.EnableRaisingEvents = true;
                    p.Start();
                    fileItem.ProcessRef = p;
                    p.Exited += (object s, EventArgs ea) =>
                    {
                        fileItem.ProcessRef = null;
                        File.Delete(patchedFile);
                    };
                }
            }

        }

        private void filesList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (filesList.SelectedIndex != -1)
                {
                    var fileItem = filesList.Items[filesList.SelectedIndex] as FileListItem;
                    if (fileItem != null)
                    {
                        fileItem.Marked = !fileItem.Marked;
                    }
                }
            }
        }
    }
}
