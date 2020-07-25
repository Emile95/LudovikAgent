using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Library.Encodable
{
    [Serializable]
    public class Log
    {
        [Serializable]
        public enum Type
        {
            Info,
            Error
        }

        public Type type;
        public string message;

        
    }
}
