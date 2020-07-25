namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Server server = new Server.Server();
            server.Start();

            bool runApp = true;
            while (runApp)
            {
                string cmd = System.Console.ReadLine();
                switch (cmd)
                {
                    case "quit":
                        runApp = false;
                        break;
                }
            }
        }
    }
}
            /*
            //Bind an valid ipAddres
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 300));

            string port = ((IPEndPoint)_serverSocket.LocalEndPoint).Port.ToString();
            System.Console.WriteLine("Agent Running on port : " + ((IPEndPoint)_serverSocket.LocalEndPoint).Port.ToString());
            File.WriteAllText(Directory.GetCurrentDirectory()+"\\port", port);

            //Max of client socket
            _serverSocket.Listen(0);

            try
            {
                //Enable the server socket to accept new client socket
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            } 
            catch(SocketException e) 
            {
            }

            while (true) { }
            
        }*/
        /*
        private static void AcceptCallBack(IAsyncResult result)
        {
            //Add the client socket in list and stop accept new socket
            Socket socket = _serverSocket.EndAccept(result);
            byte[] buffer = new byte[1024];

            Tuple < Socket, Dictionary<string, byte[]> >  tuple = new Tuple<Socket, Dictionary<string, byte[]>>(socket, new Dictionary<string, byte[]>());
            tuple.

            _clientSockets.Add(tuple);

            //Enable this socket to receive Data
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);

            //Reanable the server socket to accept new client socket
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        private static void ReceiveCallBack(IAsyncResult result)
        {
            //this socket
            Socket socket = (Socket)result.AsyncState;
            byte[] socketBuffer = new byte[1024];

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
                serializedObject = bf.Deserialize(memorystream);
            }

            Application app = null;
            if (serializedObject is ServerInstance<ProcessRunInfo>)
            {
                ServerInstance<ProcessRunInfo> instance = serializedObject as ServerInstance<ProcessRunInfo>;
                app = new ProcessRunner(
                    instance.obj,
                    socket,
                    instance.key
                );
            }

            else if (serializedObject is ConsoleLog)
                System.Console.WriteLine((serializedObject as ConsoleLog).log);

            if(app != null) app.Run();

            //Reanable this socket to receive data
            socket.BeginReceive(socketBuffer, 0, socketBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
        }*

        
        private static void SendCallBack(IAsyncResult result)
        {
            //this socket
            Socket socket = (Socket)result.AsyncState;

            //send data to the connected remote
            socket.EndSend(result);
        }
    }
}*/
