using System;

namespace OnkyoISCPlib.Commands {
  public class Input : ISCPPacket {
    /*"SLI" - Input Selector Command 	
      "00"	sets VIDEO1    VCR/DVR
      "01"	sets VIDEO2    CBL/SAT
      "02"	sets VIDEO3    GAME/TV    GAME
      "03"	sets VIDEO4    AUX1(AUX)
      "04"	sets VIDEO5    AUX2
      "05"	sets VIDEO6    PC
      "06"	sets VIDEO7
      "07"	Hidden1
      "08"	Hidden2
      "09"	Hidden3
      "10"	sets DVD          BD/DVD
      "20"	sets TAPE(1)    TV/TAPE
      "21"	sets TAPE2
      "22"	sets PHONO
      "23"	sets CD    TV/CD
      "24"	sets FM
      "25"	sets AM
      “26”	sets TUNER
      "27"	sets MUSIC SERVER    P4S   DLNA*2
      "28"	sets INTERNET RADIO           iRadio Favorite*3
      "29"	sets USB/USB(Front)
      "2A"	sets USB(Rear)
      "2B"	sets NETWORK                      NET
      "2C"	sets USB(toggle)
      "40"	sets Universal PORT
      "30"	sets MULTI CH
      "31"	sets XM*1
      "32"	sets SIRIUS*1
      "UP"	sets Selector Position Wrap-Around Up
      "DOWN"	sets Selector Position Wrap-Around Down
      "QSTN"	gets The Selector Position
      */
    public Input(string command, string displayName)
      : base(command) {
        InputName = displayName;
    }
    public Input(string command)
      : base(command) {
    }

    [ISCPCmdAttrib("!1SLI00")]
    public static ISCPPacket Video1 { get { return new Input("!1SLI00", "VCR/DVR"); } }

    [ISCPCmdAttrib("!1SLI01")]
    public static Input Video2 { get { return new Input("!1SLI01", "CBL/SAT"); } }

    [ISCPCmdAttrib("!1SLI02")]
    public static Input Video3 { get { return new Input("!1SLI02", "GAME"); } }

    [ISCPCmdAttrib("!1SLI03")]
    public static Input Video4 { get { return new Input("!1SLI03", "AUX1"); } }

    [ISCPCmdAttrib("!1SLI04")]
    public static Input Video5 { get { return new Input("!1SLI04", "AUX2"); } }

    [ISCPCmdAttrib("!1SLI05")]
    public static Input Video6 { get { return new Input("!1SLI05", "PC"); } }

    [ISCPCmdAttrib("!1SLI06")]
    public static Input Video7 { get { return new Input("!1SLI06", "VIDEO 7"); } }

    [ISCPCmdAttrib("!1SLI07")]
    public static Input Hidden1 { get { return new Input("!1SLI07", "HIDDEN 1"); } }

    [ISCPCmdAttrib("!1SLI08")]
    public static Input Hidden2 { get { return new Input("!1SLI08", "HIDDEN 2"); } }

    [ISCPCmdAttrib("!1SLI09")]
    public static Input Hidden3 { get { return new Input("!1SLI09", "HIDDEN 3"); } }

    [ISCPCmdAttrib("!1SLI10")]
    public static Input DVD { get { return new Input("!1SLI10", "BD/DVD"); } }

    [ISCPCmdAttrib("!1SLI20")]
    public static Input Tape1 { get { return new Input("!1SLI20", "TV/TAPE(1)"); } }

    [ISCPCmdAttrib("!1SLI21")]
    public static Input Tape2 { get { return new Input("!1SLI21", "TAPE 2"); } }

    [ISCPCmdAttrib("!1SLI22")]
    public static Input Phono { get { return new Input("!1SLI22", "PHONO"); } }

    [ISCPCmdAttrib("!1SLI23")]
    public static Input CD { get { return new Input("!1SLI23", "TV/CD"); } }

    [ISCPCmdAttrib("!1SLI24")]
    public static Input FM { get { return new Input("!1SLI24", "FM"); } }

    [ISCPCmdAttrib("!1SLI25")]
    public static Input AM { get { return new Input("!1SLI25", "AM"); } }

    [ISCPCmdAttrib("!1SLI26")]
    public static Input Tuner { get { return new Input("!1SLI26", "TUNER"); } }

    [ISCPCmdAttrib("!1SLI27")]
    public static Input DLNA { get { return new Input("!1SLI27", "DLNA"); } }

    [ISCPCmdAttrib("!1SLI28")]
    public static Input InternetRadio { get { return new Input("!1SLI28", "INTERNET RADIO"); } }

    [ISCPCmdAttrib("!1SLI29")]
    public static Input FrontUSB { get { return new Input("!1SLI29", "FRONT USB"); } }

    [ISCPCmdAttrib("!1SLI2A")]
    public static Input RearUSB { get { return new Input("!1SLI2A", "REAR USB"); } }

    [ISCPCmdAttrib("!1SLI2B")]
    public static Input Network { get { return new Input("!1SLI2B", "NET"); } }

    [ISCPCmdAttrib("!1SLIUP")]
    public static Input Next { get { return new Input("!1SLIUP", "Next input"); } }

    [ISCPCmdAttrib("!1SLIDOWN")]
    public static Input Previous { get { return new Input("!1SLIDOWN", "Previous input"); } }

    [ISCPCmdAttrib("!1SLIQSTN")]
    public static Input Status { get { return new Input("!1SLIQSTN", "Status"); } }

    public string InputName { get; set; }
    new public static Input ParsePacket(string command) {
      Input i = default(Input);
      Type type = typeof(Input); // MyClass is static class with static properties
      foreach (var p in type.GetProperties()) {
        var v = p.GetCustomAttributes(typeof(ISCPCmdAttrib), false);
        if (((ISCPCmdAttrib)v[0]).Name == command)
          return (Input)p.GetValue(i, null);
      }
      throw new ArgumentException("Cannot parse " + command, "command");
    }

    public override string ToString() {
      return string.Format("{0}",
        InputName);
    }
  }
}
