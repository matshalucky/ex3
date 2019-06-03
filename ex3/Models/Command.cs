using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
namespace ex3.Models
{

    class Commands
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader streamReader;  
        private bool isConnected = false;
        private bool isProgramAlive = true;
        private static Commands m_Instance = null;
        private static Mutex mutex = new Mutex();
        private Thread connection;
        // singleton
        public static Commands Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Commands();
                }
                return m_Instance;
            }
        }
        // change in case of dissconnect button is active 
        public bool IsProgramAlive
        {
            get
            {
                return isProgramAlive;
            }
            set
            {
                mutex.WaitOne();
                isProgramAlive = value;
                mutex.ReleaseMutex();
            }
        }

       
        public void Connect(string ip, int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            client = new TcpClient();
            // try to connect if the program is alive 
            while (true)
            {
                if (!IsProgramAlive)
                {
                    return;
                }
                if (client.Connected)
                {
                    break;
                }
                try { client.Connect(ep); }
                catch (Exception) { }
            }
            isConnected = true;
            stream = client.GetStream();
            streamReader = new StreamReader(stream);
        }
        // send a comman to simulaton if connected
        private string ParseData(string data)
        {
            string[] words = data.Split('\'');
            return words[1];
        }
        public string GetData(string command)
        {
            string data;
            if (!isConnected)
            {
                return "not connect";
            }
            string binaryCommand = command + "\r\n";
            byte[] bufferRoWrite = Encoding.ASCII.GetBytes(binaryCommand);
            stream.Write(bufferRoWrite, 0, bufferRoWrite.Length);
            data =streamReader.ReadLine();
            return ParseData(data);
        }

       
        // close the serever
        public void Close()
        {
            isConnected = false;
            client.Close();
        }
    }
}
