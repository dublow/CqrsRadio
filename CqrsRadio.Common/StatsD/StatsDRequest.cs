using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CqrsRadio.Common.StatsD
{
    public class StatsDRequest : IStatsDRequest
    {
        private readonly string _host;
        private readonly int _port;

        public StatsDRequest(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void Send(string value)
        {
            var socket = new Socket(
                AddressFamily.InterNetwork, 
                SocketType.Dgram,
                ProtocolType.Udp);

            var ipAddress = IPAddress.Parse(_host);

            var endPoint = new IPEndPoint(ipAddress, _port);

            var send_buffer = Encoding.ASCII.GetBytes(value);

            socket.SendTo(send_buffer, endPoint);
        }
    }
}
