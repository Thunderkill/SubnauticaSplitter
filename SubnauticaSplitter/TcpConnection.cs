using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace SubnauticaSplitter
{
    /// <summary>
    /// This class handles the connection between the splitter and LiveSplit Server
    /// </summary>
    public class TcpConnection
    {
        private Socket _client;

        public bool Connected => _client != null && _client.Connected;

        IPEndPoint _endPoint = new IPEndPoint(IPAddress.Loopback, 16834);

        public TcpConnection()
        {
            _client = new Socket(_endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _client.Connect(_endPoint);
            }
            catch (Exception e)
            {
                Debug.LogError("[SubnauticaSplitter]: Could not establish connection to LiveSplit Server " + e);
            }
        }

        /// <summary>
        /// Send's string data to the LiveSplit Server
        /// </summary>
        /// <param name="message"></param>
        private void Send(string message)
        {
            if (_client == null || !_client.Connected)
                return;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message + "\r\n");
                _client.Send(bytes);
            }
            catch (Exception e)
            {
                Debug.LogError("[SubnauticaSplitter]: Could not send packet data " + e);
            }
        }

        /// <summary>
        /// Sends the current game time to the LiveSplit Server
        /// </summary>
        /// <param name="time">Current Game Time</param>
        public void SendGametime(float time)
        {
            Send("setgametime " + TimeSpan.FromSeconds(time));
        }

        /// <summary>
        /// Sends a command to start the timer on LiveSplit
        /// </summary>
        public void StartRun()
        {
            Send("starttimer");
        }

        /// <summary>
        /// Sends command to pause Game Time on LiveSplit
        /// </summary>
        public void PauseGametime()
        {
           Send("pausegametime");
        }

        /// <summary>
        /// Sends command to resume Game Time on LiveSplit
        /// </summary>
        public void ResumeGametime()
        {
            Send("unpausegametime");
        }

        /// <summary>
        /// Sends command to end the run on LiveSplit
        /// </summary>
        public void Split()
        {
            Send("split");
        }
    }
}