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
    public abstract class UdpServer
    {
        byte[] data;
        Thread networkThread;
        UdpClient socket;
        IPEndPoint sender;

        public event EventHandler<EventArgs> DataReceived;

        protected void OnDataReceived(EventArgs e)
        {
            EventHandler<EventArgs> dataReceived = DataReceived;
            if (dataReceived != null)
                dataReceived(this, e);
        }


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
                ProcessData(data);
                OnDataReceived(EventArgs.Empty);
            }
        }

        protected abstract void ProcessData(byte[] data);
    }
}
