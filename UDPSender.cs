using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPSender
{
    private UdpClient client;
    private IPEndPoint endPoint;

    public UDPSender(string ip = "127.0.0.1", int port = 9000)
    {
        client = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
    }

    public void Send(float value)
    {
        string message = value.ToString("F3", System.Globalization.CultureInfo.InvariantCulture);
        byte[] data = Encoding.UTF8.GetBytes(message);
        client.Send(data, data.Length, endPoint);
    }

    public void Close()
    {
        client.Close();
    }
}