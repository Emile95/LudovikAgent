using Library.Application.ProcessRunner;
using Library.Encodable;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    public class ClientSocket
    {
        private readonly Socket _socket;
        private readonly byte[] _buffer;
        
        public ClientSocket(Socket socket)
        {
            _socket = socket;
            _buffer = new byte[1024];
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), _socket);
        }

        private  void ReceiveCallBack(IAsyncResult result)
        {
            //this socket
            Socket socket = (Socket)result.AsyncState;

            //stop this socket to receive data and get length of byte
            int received = socket.EndReceive(result);

            //Copy the socket buffer who contain command in new buffer
            byte[] buffer = new byte[received];
            Array.Copy(
                _buffer,
                buffer,
                received
            );

            //Result of the command
            object serializedObject;

            using (MemoryStream memorystream = new MemoryStream(buffer))
            {
                BinaryFormatter bf = new BinaryFormatter();
                serializedObject = bf.Deserialize(memorystream);
            }

            if (serializedObject is ConsoleLog)
                System.Console.WriteLine((serializedObject as ConsoleLog).log);

            else if (serializedObject is ServerInstance<ProcessRunInfo>)
            {
                System.Console.WriteLine("asdasd");
                ServerInstance<ProcessRunInfo> instance = serializedObject as ServerInstance<ProcessRunInfo>;

                ProcessRunner processRunner = new ProcessRunner(
                    instance.obj,
                    socket,
                    instance.key
                );
                processRunner.Run();
            }

            //Reanable this socket to receive data
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
        }
    }
}
