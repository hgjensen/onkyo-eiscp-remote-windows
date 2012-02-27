using System;
using System.Net.NetworkInformation;
using System.Threading;
using OnkyoISCPlib;
using OnkyoISCPlib.Commands;

namespace ConsoleApplication1 {
  class Program {
    static void Main(string[] args) {
      Console.Write("Trying to find Onkyo reciever...");
      string deviceip = ISCPDeviceDiscovery.DiscoverDevice("172.16.40.255", 60128);
      if (deviceip == string.Empty) {
        Console.WriteLine(" failed.");
        Console.Write("Please input IP of reciever: ");
        deviceip = Console.ReadLine();
      } else {
        Console.WriteLine(" Success.");
      }

      Ping p = new Ping();
      PingReply rep = p.Send(deviceip, 3000);
      while (rep.Status != IPStatus.Success) {
        Console.WriteLine("Cannot connect to Onkyo reciever ({0}). Sleeping 30sec", rep.Status);
        Thread.Sleep(30000);
        p.Send(deviceip, 3000);
      }

      Console.WriteLine("Connecting.");
      ISCPSocket.DeviceIp = deviceip;
      ISCPSocket.DevicePort = 60128;
      ISCPSocket.OnPacketRecieved += ISCPSocket_OnPacketRecieved;
      ISCPSocket.StartListener();

      writeMenu();

      bool shouldstop = false;
      while (!shouldstop) {
        switch (Console.ReadKey(true).Key) {
          case ConsoleKey.C:
            Console.Write("Host-IP:");
            string input = Console.ReadLine();
            Console.Write("Port:");
            int port = Convert.ToInt32(Console.ReadLine());

            break;
          case ConsoleKey.Add:
          case ConsoleKey.OemPlus:
            ISCPSocket.SendPacket(MasterVolume.Up);
            break;
          case ConsoleKey.Subtract:
          case ConsoleKey.OemMinus:
            ISCPSocket.SendPacket(MasterVolume.Down);
            break;
          case ConsoleKey.V:
            ISCPSocket.SendPacket(MasterVolume.Status);
            break;
          case ConsoleKey.P:
            ISCPSocket.SendPacket(Power.Status);
            break;
          case ConsoleKey.M:
            ISCPSocket.SendPacket(Muting.Toggle);
            break;
          case ConsoleKey.N:
            ISCPSocket.SendPacket(Muting.Status);
            break;
          case ConsoleKey.H:
            ISCPSocket.SendPacket(OSD.Home);
            break;
          case ConsoleKey.UpArrow:
            ISCPSocket.SendPacket(OSD.Up);
            break;
          case ConsoleKey.DownArrow:
            ISCPSocket.SendPacket(OSD.Down);
            break;
          case ConsoleKey.RightArrow:
            ISCPSocket.SendPacket(OSD.Right);
            break;
          case ConsoleKey.LeftArrow:
            ISCPSocket.SendPacket(OSD.Left);
            break;
          case ConsoleKey.X:
            ISCPSocket.SendPacket(OSD.Exit);
            break;
          case ConsoleKey.Enter:
            ISCPSocket.SendPacket(OSD.Enter);
            break;
          case ConsoleKey.Q:
            shouldstop = true;
            break;
        }
      }

      Console.WriteLine("Done. Press any key to exit.");
      Console.ReadLine();
      ISCPSocket.Dispose();
    }

    static void ISCPSocket_OnPacketRecieved(string str) {
      Console.Clear();
      writeMenu();
      Console.WriteLine("Recieved: " + str);
      var r = ISCPPacket.ParsePacket(str);
      Console.WriteLine(r.ToString());
    }

    private static void writeMenu() {
      Console.Write(@"Menu:
 Set Volume   +/-
 Get Volume   V
 Toggle mute  M
 Get mute     N
 Get Power    P
 Quit         Q

 Home         H
 Exit         X
 Enter        Enter
 Navigate     Arrow-keys

"
        );
    }
  }
}
