using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace OnkyoISCPlib {
  public delegate void PacketRecieved(string str);

  public static class ISCPSocket {
    private static bool _shutdown;
    private static Socket _sock;
    private static Thread _listener;
    public static string DeviceIp { get; set; }
    public static int DevicePort { get; set; }
    private static bool blocked { get; set; }
    public static event PacketRecieved OnPacketRecieved;

    public static void SendPacket(ISCPPacket packet, bool blocking = false) {
      if (blocking) {
        blocked = true;
        OnPacketRecieved -= blockingListen;
        OnPacketRecieved += blockingListen;
      }
      checkConnect();
      if (_sock != null && _sock.Connected) {
        _sock.Send(packet.GetBytes(), 0, packet.GetBytes().Length, SocketFlags.None);
      }
      while (blocked) Thread.Sleep(100);
      Thread.Sleep(100);
    }

    public static void StartListener() {
      checkConnect();
      _shutdown = false;
      _listener = new Thread(socketListener);
      _listener.Start();
    }

    public static void StopListener() {
      _shutdown = true;
    }

    public static void Dispose() {
      _shutdown = true;
      _sock.Close();
      _sock.Dispose();
      try {
        _listener.Abort();
      } catch {
      }
    }

    private static void socketListener() {
      try {
        while (!_shutdown) {
          try {
            if (_sock.Available > 0) {
              var buffer = new byte[256];
              _sock.Receive(buffer, buffer.Length, SocketFlags.None);
              var builder = new StringBuilder();
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
      try {
        if (_sock == null)
          _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 1000 };
        if (!_sock.Connected)
          _sock.Connect(DeviceIp, DevicePort);
      } catch (Exception x) {
        
      }
    }

    private static void blockingListen(string str) {
      blocked = false;
    }
  }
}