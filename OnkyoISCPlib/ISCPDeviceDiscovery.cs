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

    public static DiscoveryResult DiscoverDevice(string networkaddress, int port) {
      DiscoveryResult ret = new DiscoveryResult();
      localEndPoint = new IPEndPoint(IPAddress.Any, 0);
      client = new UdpClient(port);

      var p = new ISCPPacket("!xECNQSTN");
      byte[] sendbuf = p.GetBytes();
      client.Send(sendbuf, sendbuf.Length, IPAddress.Parse(networkaddress).ToString(), port);
      client.Send(sendbuf, sendbuf.Length, IPAddress.Parse(networkaddress).ToString(), port);
      client.Send(sendbuf, sendbuf.Length, IPAddress.Parse(networkaddress).ToString(), port);
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
          ret.IP = ip;
          ret.Port = Convert.ToInt32(stringData.Substring(idx + 8, 5));
          ret.Region = stringData.Substring(idx + 14, 2);
          ret.MAC = mac;
          ret.Model = stringData.Substring(idx, 7);
        }
      }
      client.Close();
      return ret;
    }
    
    //"!cECNnnnnnn/ppppp/dd/iiiiiiiiiiii"
    //dd:
    // DX: North American model
    // XX: European or Asian model
    // JJ: Japanese model

    public struct DiscoveryResult {
      public string MAC { get; set; }
      public string IP { get; set; }
      public int Port { get; set; }
      public string Model { get; set; }
      public string Region { get; set; }
      public override string ToString() {
        return string.Format("Model: {0}, Region: {1}, MAC: {2}\r\nIP: {3}, Port: {4}",
          Model,
          (Region == "DX") ? "North America" : (Region == "XX") ? "Europe/Asia" : (Region == "N/A") ? "N/A" : "Japan",
          MAC,
          IP,
          Port);
      }
    }
  }
}
