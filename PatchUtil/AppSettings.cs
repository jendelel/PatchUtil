using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace PatchUtil
{
    [DataContract]
    class AppSettings
    {
        [IgnoreDataMember]
        private static AppSettings instance;

        private AppSettings() { }

        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    using (FileStream fis = new FileStream("settings.json", FileMode.Open, FileAccess.Read))
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(AppSettings));
                        instance = (AppSettings)ser.ReadObject(fis);
                    }
                }
                return instance;
            }
        }

        [DataMember]
        public string PythonPath { get; set; }
        [DataMember]
        public string DiffToolPath { get; set; }
    }
}
