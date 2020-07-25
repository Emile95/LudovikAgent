using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Server
    {
        #region Propeties and Constructor

        private readonly Socket _socket;
        private List<ClientSocket> _clients;

        public Server()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion

        #region Private Methods

        private void DisplayPort()
        {
            string port = ((IPEndPoint)_socket.LocalEndPoint).Port.ToString();
            System.Console.WriteLine("Agent Running on port : " + ((IPEndPoint)_socket.LocalEndPoint).Port.ToString());
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\port", port);
        }

        private void AcceptCallBack(IAsyncResult result)
        {
            _clients.Add(new ClientSocket(_socket.EndAccept(result)));
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            _clients = new List<ClientSocket>();
            
            _socket.Bind(new IPEndPoint(IPAddress.Any, 300));
            _socket.Listen(0);

            DisplayPort();

            _socket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        /*
        public void Stop()
        {
            _clients.ForEach(o => o.Stop());
            
            try
            {
                _socket.Disconnect(true);
            }
            catch (Exception e) { }
        }*/

        #endregion
    }
}
