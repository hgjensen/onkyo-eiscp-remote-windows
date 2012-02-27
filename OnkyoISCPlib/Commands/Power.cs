using System;

namespace OnkyoISCPlib.Commands {
  public class Power : ISCPPacket {
    /*"PWR" - System Power Command  	
      "00"	sets System Standby
      "01"	sets System On
      "QSTN"	gets the System Power Status
      */
    public static Power On { get { return new Power("!1PWR01", "On"); } }
    public static Power Off { get { return new Power("!1PWR00", "Standby"); } }
    public static Power Status { get { return new Power("!1PWRQSTN", "Status"); } }

    public string Name { get; set; }

    public Power(string command, string name)
      : base(command) {
        Name = name;
    }

    public static Power ParsePacket(string command) {
      switch (command) {
        case "00": return Off;
        case "01": return On;
        case "QSTN": return Status;
      }
      throw new ArgumentException("Cannot find the command", "command");
    }

    public override string ToString() {
      return string.Format("System power is {0}", Name);
    }
  }
}
