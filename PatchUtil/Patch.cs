using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchUtil
{
    class Patch
    {
        string patchPath;
        public Patch(string filePath)
        {
            this.patchPath = filePath;
        }

        public List<string> GetFileList()
        {
            List<string> list = new List<string>();
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = AppSettings.Instance.PythonPath,
                    Arguments = $"patch_wrapper.py --cmd=ls --patch_file=\"{patchPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            p.Start();
            while (!p.StandardOutput.EndOfStream)
            {
                string line = p.StandardOutput.ReadLine();
                list.Add(line);
            }
            return list;
        }


        public FileInfo PatchFileInfo
        {
            get
            {
                return new FileInfo(patchPath);
            }
        }


        public bool ApplyPatch(string file, string targetFilePath)
         {
         using (Process p = new Process
            {
            StartInfo = new ProcessStartInfo
               {
               FileName = AppSettings.Instance.PythonPath,
               Arguments = $"patch_wrapper.py --cmd=apply --patch_file=\"{patchPath}\" --orig_file=\"{file}\" --target_file=\"{targetFilePath}\"",
               UseShellExecute = false,
               RedirectStandardOutput = true,
               RedirectStandardError = true,
               CreateNoWindow = true
               }
            })
            {
            p.OutputDataReceived += (object s, DataReceivedEventArgs e) => { Debug.WriteLine(e.Data); };
            p.ErrorDataReceived += (object s, DataReceivedEventArgs e) => { Debug.WriteLine(e.Data); };
            p.Start();
            p.WaitForExit();
            return p.ExitCode == 0;
            }
         }
      }


}
