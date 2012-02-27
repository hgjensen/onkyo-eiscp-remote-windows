using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace OnkyoISCPlib {
  public static class ISCPDeviceDiscovery {
    /* "!xECNQSTN" */
    static UdpClient client;
    static IPEndPoint localEndPoint;

    public static string DiscoverDevice(string networkaddress, int port) {
      localEndPoint = new IPEndPoint(IPAddress.Any, 0);
      client = new UdpClient(port);

      var p = new ISCPPacket("!xECNQSTN");
      byte[] sendbuf = p.GetBytes();
      client.Send(sendbuf, sendbuf.Length, IPAddress.Parse(networkaddress).ToString(), port);
      client.Send(sendbuf, sendbuf.Length, IPAddress.Parse(networkaddress).ToString(), port);
      client.Send(sendbuf, sendbuf.Length, IPAddress.Parse(networkaddress).ToString(), port);
      string ret = string.Empty;
      while (client.Available > 0) {
        byte[] recv = client.Receive(ref localEndPoint);
        Thread.Sleep(100);
        StringBuilder sb = new StringBuilder();
        for (int x = 0; x < recv.Length; x++)
          sb.Append(char.ConvertFromUtf32(Convert.ToInt32(string.Format("{0:x2}", recv[x]), 16)));
        string stringData = sb.ToString();
        if (stringData.Contains("!1ECN")) {
          int idx = stringData.IndexOf("!1ECN") + 5;
          string mac = stringData.Substring(idx + 17, 12);
          string ip = ARP.GetIPInfo(mac).IPAddress;
          ret = ip;
          Console.WriteLine("Model: " + stringData.Substring(idx, 6));
          Console.WriteLine("MAC: " + mac);
          Console.WriteLine("IP: " + ip);
        }
      }
      client.Close();
      return ret;
    }
    //"!cECNnnnnnn/ppppp/dd/iiiiiiiiiiii"
  }
}
