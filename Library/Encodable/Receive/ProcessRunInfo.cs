using System;

namespace Library.Encodable
{
    [Serializable]
    public class ProcessRunInfo
    {
        public string fileName;
        public string args;
        public string workingDirectory;
        public string vars;
    }
}
