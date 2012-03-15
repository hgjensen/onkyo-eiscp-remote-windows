using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using HGJ.ConsoleLib;
using OnkyoISCPlib;
using OnkyoISCPlib.Commands;

namespace ConsoleApplication1 {
  class Program {
    private static bool powerStatus;
    private static string inputStatus;

    static void Main(string[] args) {
      //Setup console-regions
      HGJConsole.Reset();
      HGJConsole.BackgroundColor = ConsoleColor.Blue;
      HGJConsole.Regions.Add("input1", new ConsoleRegion(new ConsolePoint(1, 1), 4, 35, "Input", true));
      HGJConsole.Regions.Add("recv", new ConsoleRegion(new ConsolePoint(1, 6), 4, 35, "Recieved", false));
      HGJConsole.Regions.Add("status", new ConsoleRegion(new ConsolePoint(1, 11), 3, 35, "Status", true));
      HGJConsole.Regions.Add("menu", new ConsoleRegion(new ConsolePoint(40, 1), 13, 35, "Menu", false));
      HGJConsole.Regions.Add("device", new ConsoleRegion(new ConsolePoint(1, 15), 3, 74, "Device-info", false));
      HGJConsole.Regions.Add("inputselect", new ConsoleRegion(new ConsolePoint(7, 2), 16, 60, "Input-select", false));
      HGJConsole.Draw(true);

      //Auto-discovery, or get IP by input
      HGJConsole.Regions["status"].WriteContent("Finding reciever...");
      //var discovery = ISCPDeviceDiscovery.DiscoverDevice("172.16.40.255", 60128);
      var discovery = ISCPDeviceDiscovery.DiscoverDevice(60128);
      string deviceip = discovery.IP;
      if (string.IsNullOrEmpty(deviceip)) {
        HGJConsole.Regions["status"].WriteContent("Finding reciever... failed.");
        HGJConsole.Regions["input1"].WriteContent("Please input IP of reciever: ");
        deviceip = HGJConsole.Regions["input1"].GetLine(2);
        HGJConsole.Regions["device"].Visible = true;
        discovery.IP = deviceip;
        discovery.MAC = "N/A";
        discovery.Model = "N/A";
        discovery.Port = 60128;
        discovery.Region = "N/A";
        HGJConsole.Regions["device"].WriteContent(discovery.ToString());
      } else {
        HGJConsole.Regions["status"].WriteContent("Finding reciever... Success.");
        HGJConsole.Regions["device"].Visible = true;
        HGJConsole.Regions["device"].WriteContent(discovery.ToString());
      }

      //Check if host is alive
      Ping p = new Ping();
      PingReply rep = p.Send(deviceip, 3000);
      while (rep.Status != IPStatus.Success) {
        HGJConsole.Regions["status"].WriteContent(string.Format("Cannot connect to Onkyo reciever ({0}). Sleeping 30sec", rep.Status));
        Thread.Sleep(30000);
        p.Send(deviceip, 3000);
      }

      //Setup sockets to reciever
      HGJConsole.Regions["status"].WriteContent("Connecting.");
      ISCPSocket.DeviceIp = discovery.IP;
      ISCPSocket.DevicePort = discovery.Port;
      ISCPSocket.OnPacketRecieved += ISCPSocket_OnPacketRecieved;
      ISCPSocket.StartListener();
      HGJConsole.Regions["status"].WriteContent("Connected!");
      HGJConsole.Regions["recv"].Visible = true;
      HGJConsole.Regions["input1"].WriteContent("Input:\r\n> ");

      //Write menu to console-region
      writeMenu();

      //Loop input characters...
      bool shouldstop = false;
      while (!shouldstop) {
        var cki = HGJConsole.Regions["input1"].GetChar(2);
        if (cki.Modifiers == ConsoleModifiers.Shift) {
          switch (cki.Key) {
            case ConsoleKey.Add:
            case ConsoleKey.OemPlus:
            case ConsoleKey.Subtract:
            case ConsoleKey.OemMinus:
              ISCPSocket.SendPacket(MasterVolume.Status);
              break;
            case ConsoleKey.V:
              ISCPSocket.SendPacket(MasterVolume.Status);
              break;
            case ConsoleKey.P:
              ISCPSocket.SendPacket(Power.Status);
              break;
            case ConsoleKey.M:
              ISCPSocket.SendPacket(Muting.Status);
              break;
            case ConsoleKey.A:
              ISCPSocket.SendPacket(Audio.Status);
              break;
          }
        } else {
          switch (cki.Key) {
            case ConsoleKey.Add:
            case ConsoleKey.OemPlus:
              ISCPSocket.SendPacket(MasterVolume.Up);
              break;
            case ConsoleKey.Subtract:
            case ConsoleKey.OemMinus:
              ISCPSocket.SendPacket(MasterVolume.Down);
              break;
            case ConsoleKey.I:
              inputMenu();
              break;
            case ConsoleKey.P:
              ISCPSocket.SendPacket(Power.Status, true);
              ISCPSocket.SendPacket(powerStatus ? Power.Off : Power.On);
              break;
            case ConsoleKey.M:
              ISCPSocket.SendPacket(Muting.Toggle);
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
      }

      HGJConsole.Regions["status"].WriteContent("... Press any key to exit ...");
      Console.ReadKey();
      ISCPSocket.Dispose();
    }

    static void ISCPSocket_OnPacketRecieved(string str) {
      HGJConsole.Regions["recv"].WriteContent("Recieved: " + str);
      var r = ISCPPacket.ParsePacket(str);
      if (r is Power) {
        powerStatus = (r.Command == "!1PWR01");
      } else if (r is Input) {
        inputStatus = r.ToString();
      }
      HGJConsole.Regions["recv"].WriteContent(r.ToString(), true);
    }

    private static void writeMenu() {
      if (!HGJConsole.Regions["menu"].Visible) HGJConsole.Regions["menu"].Visible = true;
      HGJConsole.Regions["menu"].WriteContent(@"            Action:     Status:
 Audio-info             Shift A
 Volume     +/-         Shift +/-
 Mute       M           Shift M
 Power      P           Shift P
 Quit       Q
 Input                  I

 Home       H
 Exit       X
 Enter      Enter
 Navigate   Arrow-keys

"
        );
    }

    private static void inputMenu() {
      ISCPSocket.SendPacket(Input.Status);
      Thread.Sleep(250);
      if (!HGJConsole.Regions["inputselect"].Visible) HGJConsole.Regions["inputselect"].Visible = true;
      writeInputMenu();
      ConsoleKeyInfo cki = default(ConsoleKeyInfo);
      while (cki.Key != ConsoleKey.Escape) {
        cki = HGJConsole.Regions["inputselect"].GetChar(14);
        switch (cki.Key) {
          case ConsoleKey.V:
            ISCPSocket.SendPacket(Input.Video1);
            break;
          case ConsoleKey.S:
            ISCPSocket.SendPacket(Input.Video2);
            break;
          case ConsoleKey.G:
            ISCPSocket.SendPacket(Input.Video3);
            break;
          case ConsoleKey.A:
            ISCPSocket.SendPacket(Input.Video4);
            break;
          case ConsoleKey.B:
            ISCPSocket.SendPacket(Input.Video5);
            break;
          case ConsoleKey.P:
            ISCPSocket.SendPacket(Input.Video6);
            break;
          case ConsoleKey.D:
            ISCPSocket.SendPacket(Input.DVD);
            break;
          case ConsoleKey.T:
            ISCPSocket.SendPacket(Input.Tape1);
            break;
          case ConsoleKey.H:
            ISCPSocket.SendPacket(Input.Phono);
            break;
          case ConsoleKey.C:
            ISCPSocket.SendPacket(Input.CD);
            break;
          case ConsoleKey.F:
            ISCPSocket.SendPacket(Input.FM);
            break;
          case ConsoleKey.L:
            ISCPSocket.SendPacket(Input.DLNA);
            break;
          case ConsoleKey.N:
            ISCPSocket.SendPacket(Input.Network);
            break;
        }
        writeInputMenu();
      }
      HGJConsole.Regions["inputselect"].Visible = false;
      HGJConsole.Draw(true);
    }
    private static void writeInputMenu() {
      HGJConsole.Regions["inputselect"].WriteContent(string.Format(@"Current input: {0}

Key: Input:  | Key:  Input:  (ESC Return)
 V   VCR/DVD |  C  CD
 S   CBL/SAT |  F  FM
 G   GAME    |  L  DLNA
 A   AUX1    |  N  NET
 B   AUX2    |
 P   PC      |
 D   BD/TAPE |
 T   TV/TAPE |
 H   PHONO   |
", inputStatus));
    }
  }
}
