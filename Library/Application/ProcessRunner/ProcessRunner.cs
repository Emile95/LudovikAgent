using Library.Encodable;
using System.Diagnostics;
using System.Net.Sockets;

namespace Library.Application.ProcessRunner
{
    public class ProcessRunner : Application
    {
        #region Properties and Constructor

        private readonly Process _process;

        public ProcessRunner(ProcessRunInfo processRunInfo, Socket clientSocket)
        {
            _process.StartInfo.FileName = processRunInfo.fileName;
            _process.StartInfo.Arguments = processRunInfo.args;
            _process.StartInfo.WorkingDirectory = processRunInfo.workingDirectory;

            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;

            _process.OutputDataReceived += (sender, args) => 
            {
                /*
                clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, (result) => {
                    clientSocket.EndSend(result);
                }, clientSocket);
                */
            };

            _process.ErrorDataReceived += (sender, args) => 
            {
                /*
                clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, (result) => {
                    clientSocket.EndSend(result);
                }, clientSocket);
                */
            };
        }

        #endregion

        #region Application Implementation

        public sealed override void Run()
        {
            _process.Start();

            _process.BeginErrorReadLine();
            _process.BeginOutputReadLine();

            _process.WaitForExit();
        }

        #endregion
    }
}
