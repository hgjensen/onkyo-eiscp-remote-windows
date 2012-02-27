using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OnkyoISCPlib.Commands;

namespace OnkyoISCPlib {
  public class ISCPPacket {
    private const byte EOF = 0x0D;
    private static byte[] packetTemplate = new byte[] {
       0x49, 0x53, 0x43, 0x50, 
       0x00, 0x00, 0x00, 0x10, 
       0x00, 0x00, 0x00, 0xFF, //replace last with length
       0x01, 0x00, 0x00, 0x00 
       //Add data + EOF here
    };

    internal string CustomData { get; set; }

    public string Command { get; set; }

    public int Length {
      get { return cmdBytes.Length; }
    }

    private byte[] cmdBytes {
      get { return Encoding.ASCII.GetBytes(Command); }
    }

    public ISCPPacket(string command) {
      Command = command;
    }

    public byte[] GetBytes() {
      List<byte> ret = packetTemplate.ToList();
      ret[11] = byte.Parse(string.Format("{0:X2}", (Length + 1).ToString()), NumberStyles.HexNumber);
      ret.AddRange(cmdBytes);
      ret.Add(EOF);

      return ret.ToArray();
    }

    public static ISCPPacket ParsePacket(string packetstring) {
      //string direction = packetstring.Substring(1, 1);
      string group = packetstring.Substring(2, 3);
      string command = packetstring.Substring(5);
      switch (group) {
        case "MVL":
          return MasterVolume.ParsePacket(command);
        case "PWR":
          return Power.ParsePacket(command);
        case "AMT":
          return Muting.ParsePacket(command);
        case "SLI":
          break;
      }
      throw new ArgumentException("Cannot find command-group " + group, "packetstring");
    }
  }
}
