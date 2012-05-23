using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using AvengersUtd.Odyssey.Utils.Logging;
using System.Threading;

namespace AvengersUtd.Odyssey.Network
{
    public class UdpServer
    {
        byte[] data;
        Thread networkThread;
        UdpClient socket;
        IPEndPoint sender;

        public UdpServer()
        {
            data = new byte[1024];
            

            
        }

        public void Start()
        {
            
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 5555);
            socket = new UdpClient(ipep);
            
            sender = new IPEndPoint(IPAddress.Any, 0);
            LogEvent.Network.Write("Waiting for a client...");
            networkThread = new Thread(Loop) { Name = "Network" };
            networkThread.Start();
            
        }


        void Loop()
        {
            while (true)
            {
                data = socket.Receive(ref sender);
                LogEvent.Network.Write(Encoding.ASCII.GetString(data, 0, data.Length));
            }
        }
    }
}
