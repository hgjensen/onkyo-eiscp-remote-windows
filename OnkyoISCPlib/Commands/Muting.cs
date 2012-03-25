using System;

namespace OnkyoISCPlib.Commands {
  public class Muting : ISCPPacket {
    /*"AMT" - Audio Muting Command	
      "00"	sets Audio Muting Off
      "01"	sets Audio Muting On
      "TG"	sets Audio Muting Wrap-Around
      "QSTN"	gets the Audio Muting State
      */

    public Muting(string command, string name)
      : base(command) {
      Name = name;
    }

    public static Muting On {
      get { return new Muting("!1AMT01", "On"); }
    }

    public static Muting Off {
      get { return new Muting("!1AMT00", "Off"); }
    }

    public static Muting Toggle {
      get { return new Muting("!1AMTTG", "Toggle"); }
    }

    public static Muting Status {
      get { return new Muting("!1AMTQSTN", "Status"); }
    }

    public string Name { get; set; }

    public override string ToString() {
      return string.Format("Muting: " + Name);
    }

    public static new Muting ParsePacket(string command) {
      switch (command) {
        case "01":
          return On;
        case "00":
          return Off;
        case "TG":
          return Toggle;
        case "QSTN":
          return Status;
      }
      throw new ArgumentException("Cannot find the command", "command");
    }
  }
}