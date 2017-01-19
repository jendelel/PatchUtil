using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchUtil
{
    class FileListItem
    {
        public FileListItem(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; set; }

        public bool Marked { get; set; } = false;

        public Process ProcessRef { get; set; } = null;

        public override string ToString()
        {
            return Marked ? $"*  {FileName}" : FileName;
        }
    }
}
