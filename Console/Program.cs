using Library.Encodable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Console
{
    class Program
    {
        private static List<Tuple<Socket, byte[]>> _clientSockets = new List<Tuple<Socket,byte[]>>();
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            //Bind an valid ipAddres
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
            //Max of client socket
            _serverSocket.Listen(5);

            try
            {
                //Enable the server socket to accept new client socket
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            } 
            catch(SocketException e) 
            {
            }

            System.Console.Read();
        }

        private static void AcceptCallBack(IAsyncResult result)
        {
            //Add the client socket in list and stop accept new socket
            Socket socket = _serverSocket.EndAccept(result);
            byte[] buffer = new byte[1024];
            _clientSockets.Add(new Tuple<Socket, byte[]>(socket, buffer));

            //Enable this socket to receive Data
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);

            //Reanable the server socket to accept new client socket
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        private static void ReceiveCallBack(IAsyncResult result)
        {
            //this socket
            Socket socket = (Socket)result.AsyncState;
            byte[] socketBuffer = _clientSockets.Single(o => o.Item1.Equals(socket)).Item2;

            //stop this socket to receive data and get length of byte
            int received = socket.EndReceive(result);

            //Copy the socket buffer who contain command in new buffer
            byte[] buffer = new byte[received];
            Array.Copy(
                socketBuffer,
                buffer, 
                received
            );

            //Result of the command
            object serializedObject;

            using (MemoryStream memorystream = new MemoryStream(buffer))
            {
                
                BinaryFormatter bf = new BinaryFormatter();
                serializedObject = bf.Deserialize(memorystream) as ConsoleLog;
            }

            if(serializedObject is ProcessRunInfo)

            //Send send data
            //socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), socket);

            //Reanable this socket to receive data
            socket.BeginReceive(socketBuffer, 0, socketBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
        }

        /*
        private static void SendCallBack(IAsyncResult result)
        {
            //this socket
            Socket socket = (Socket)result.AsyncState;

            //send data to the connected remote
            socket.EndSend(result);
        }*/
    }
}
