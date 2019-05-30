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

        public void openClient()
        {
            connection = new Thread(delegate ()
            {
                connect("127.0.0.1", 5400);
            });
            connection.Start();
        }
        public void connect(string ip, int port)
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
        private string parseData(string data)
        {
            string[] words = data.Split('\'');
            return words[1];
        }
        public string getData(string command)
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
            return parseData(data);
        }

        // split multiline commands to sent to the simualtor
        //public void sendCommand(string userCommands)
        //{
        //    // thread so more then one command will not delay the program
        //    new Thread(delegate ()
        //    {
        //        if (!isConnected)
        //        {
        //            return;
        //        }
        //        string[] commands = userCommands.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        //        foreach (string command in commands)
        //        {
        //            (command);
        //            Thread.Sleep(2000);
        //        }

        //    }).Start();
        //}
        // close the serever
        public void close()
        {
            isConnected = false;
            client.Close();
        }
    }
}
