using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Library.Encodable
{
    [Serializable]
    public class ServerInstance<T>
    {
        public string key;
        public T obj;

        public byte[] ToBinary()
        {
            using (MemoryStream memorystream = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(memorystream, this);
                return memorystream.ToArray();
            }
        }
    }
}
