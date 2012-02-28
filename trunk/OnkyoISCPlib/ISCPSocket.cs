using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace OnkyoISCPlib {
  public delegate void PacketRecieved(string str);

  public static class ISCPSocket {
    public static event PacketRecieved OnPacketRecieved;

    public static string DeviceIp { get; set; }
    public static int DevicePort { get; set; }

    private static bool shutdown;
    private static Socket sock;
    private static Thread listener;

    public static void SendPacket(ISCPPacket packet, bool blocking = false) {
      if (blocking) {
        blocked = true;
        OnPacketRecieved -= blockingListen;
        OnPacketRecieved += blockingListen;
      }
      checkConnect();
      if (sock != null && sock.Connected) {
        sock.Send(packet.GetBytes(), 0, packet.GetBytes().Length, SocketFlags.None);
      }
      while (blocked) Thread.Sleep(100);
      Thread.Sleep(100);
    }
    public static void StartListener() {
      checkConnect();
      shutdown = false;
      listener = new Thread(socketListener);
      listener.Start();
    }
    public static void StopListener() {
      shutdown = true;
    }
    public static void Dispose() {
      shutdown = true;
      sock.Close();
      sock.Dispose();
      try { listener.Abort(); } catch { }
    }

    private static void socketListener() {
      try {
        while (!shutdown) {
          try {
            if (sock.Available > 0) {
              byte[] buffer = new byte[256];
              sock.Receive(buffer, buffer.Length, SocketFlags.None);
              StringBuilder builder = new StringBuilder();
              for (int i = 16; buffer[i] != 26; i++) {
                if (buffer[i] != 26) {
                  int num = Convert.ToInt32(string.Format("{0:x2}", buffer[i]), 16);
                  builder.Append(char.ConvertFromUtf32(num));
                }
              }
              if (OnPacketRecieved != null)
                OnPacketRecieved(builder.ToString());
            }
          } catch (Exception) {
          }
          Thread.Sleep(50);
        }
      } catch (Exception) {
      }
    }

    private static void checkConnect() {
      if (sock == null)
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 1000 };
      if (!sock.Connected)
        sock.Connect(DeviceIp, DevicePort);
    }

    private static bool blocked { get; set; }

    private static void blockingListen(string str) {
      blocked = false;
    }
  }
}
