using System;
using System.Collections.Generic;

namespace Library.Encodable
{
    [Serializable]
    public class ProcessRunInfo
    {
        public ProcessRunInfo()
        {
            //environment = new Dictionary<string, string>();
        }

        public string fileName;
        public string args;
        public string workingDirectory;
        //public Dictionary<string, string> environment;
    }
}
