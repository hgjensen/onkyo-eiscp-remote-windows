using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OnkyoISCPlib.Commands {
  public class MasterVolume : ISCPPacket {
    private const string B = "!1MVL";
    /* "MVL" - Master Volume Command 
      "00"-"64"	Volume Level 0 – 100 ( In hexadecimal representation)
      "00"-"50"	Volume Level 0 – 80 ( In hexadecimal representation)
      "UP"	sets Volume Level Up
      "DOWN"	sets Volume Level Down
      "UP1"	sets Volume Level Up 1dB Step
      "DOWN1"	sets Volume Level Down 1dB Step
      "QSTN"	gets the Volume Level
    */

    public MasterVolume(string command)
      : base(command) {
    }

    public static MasterVolume Up {
      get { return new MasterVolume(B + "UP"); }
    }

    public static MasterVolume Up1db {
      get { return new MasterVolume(B + "UP1"); }
    }

    public static MasterVolume Down {
      get { return new MasterVolume(B + "DOWN"); }
    }

    public static MasterVolume Down1db {
      get { return new MasterVolume(B + "DOWN1"); }
    }

    public static MasterVolume Status {
      get { return new MasterVolume(B + "QSTN"); }
    }

    private int lvl { get; set; }

    public static MasterVolume SetLvl(int lvl) {
      if (lvl >= 0 && lvl <= 80)
        return new MasterVolume("!1MVL" + string.Format("{0:X2}", lvl));
      throw new ArgumentException("Volume-range 0-80 (0x00-0x50 in hex)");
    }

    public static new MasterVolume ParsePacket(string command) {
      Match m = Regex.Match(command, "([A-Z0-9]{2})");
      if (m.Success) {
        MasterVolume r = SetLvl(int.Parse(command, NumberStyles.HexNumber));
        r.lvl = int.Parse(command, NumberStyles.HexNumber);
        return r;
      }
      switch (command) {
        case "UP":
          return Up;
        case "UP1":
          return Up1db;
        case "DOWN":
          return Down;
        case "DOWN1":
          return Down1db;
        case "QSTN":
          return Status;
      }
      throw new ArgumentException("Cannot find the command", "command");
    }

    public override string ToString() {
      return string.Format("Master Volume: {0}", lvl);
    }
  }
}