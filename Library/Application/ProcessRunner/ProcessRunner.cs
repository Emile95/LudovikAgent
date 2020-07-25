using Library.Encodable;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace Library.Application.ProcessRunner
{
    public class ProcessRunner : Application
    {
        #region Properties and Constructor

        private readonly Socket _socket;
        private readonly Process _process;
        private readonly string _key;

        public ProcessRunner(ProcessRunInfo processRunInfo, Socket socket, string key)
        {
            _socket = socket;
            _key = key;
            _process = new Process();
            _process.StartInfo.FileName = processRunInfo.fileName;
            _process.StartInfo.Arguments = processRunInfo.args;
            _process.StartInfo.WorkingDirectory = processRunInfo.workingDirectory;

            string[] vars = processRunInfo.vars.Split(",");

            foreach (string var in vars)
            {
                string[] varSep = var.Split(":");
                _process.StartInfo.Environment.Add(varSep[0],varSep[1]);
            }
                
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            
            _process.OutputDataReceived += (sender, args) => 
            {
                _socket.Send(
                    new ServerInstance<Log>() {
                        key = _key,
                        obj = new Log() { type = Log.Type.Info, message = args.Data }
                    }.ToBinary()
                );
            };

            _process.ErrorDataReceived += (sender, args) => 
            {
                _socket.Send(
                    new ServerInstance<Log>()
                    {
                        key = _key,
                        obj = new Log() { type = Log.Type.Error, message = args.Data }
                    }.ToBinary()
                );
            };
        }

        #endregion

        #region Application Implementation

        public sealed override void Run()
        {
            if (!Directory.Exists(_process.StartInfo.WorkingDirectory))
                Directory.CreateDirectory(_process.StartInfo.WorkingDirectory);

            _process.Start();

            _process.BeginErrorReadLine();
            _process.BeginOutputReadLine();

            _process.WaitForExit();

            /*_socket.Send(
                new ServerInstance<Log>()
                {
                    key = _key,
                    obj = new Log() { type = Log.Type.Error, message = args.Data }
                }.ToBinary()
            );*/
        }

        #endregion
    }
}
