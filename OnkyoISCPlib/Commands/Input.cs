namespace OnkyoISCPlib.Commands {
  public static class Input {
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

    [ISCPCmdAttrib("!1SLI00")]
    public static ISCPPacket Video1 { get { return new ISCPPacket("!1SLI00"); } }

    [ISCPCmdAttrib("!1SLI01")]
    public static ISCPPacket Video2 { get { return new ISCPPacket("!1SLI01"); } }

    [ISCPCmdAttrib("!1SLI02")]
    public static ISCPPacket Video3 { get { return new ISCPPacket("!1SLI02"); } }

    [ISCPCmdAttrib("!1SLI03")]
    public static ISCPPacket Video4 { get { return new ISCPPacket("!1SLI03"); } }

    [ISCPCmdAttrib("!1SLI04")]
    public static ISCPPacket Video5 { get { return new ISCPPacket("!1SLI04"); } }

    [ISCPCmdAttrib("!1SLI05")]
    public static ISCPPacket Video6 { get { return new ISCPPacket("!1SLI05"); } }

    [ISCPCmdAttrib("!1SLI06")]
    public static ISCPPacket Video7 { get { return new ISCPPacket("!1SLI06"); } }

    [ISCPCmdAttrib("!1SLI07")]
    public static ISCPPacket Hidden1 { get { return new ISCPPacket("!1SLI07"); } }

    [ISCPCmdAttrib("!1SLI08")]
    public static ISCPPacket Hidden2 { get { return new ISCPPacket("!1SLI08"); } }

    [ISCPCmdAttrib("!1SLI09")]
    public static ISCPPacket Hidden3 { get { return new ISCPPacket("!1SLI09"); } }

    [ISCPCmdAttrib("!1SLI10")]
    public static ISCPPacket DVD { get { return new ISCPPacket("!1SLI10"); } }

    [ISCPCmdAttrib("!1SLI20")]
    public static ISCPPacket Tape1 { get { return new ISCPPacket("!1SLI20"); } }

    [ISCPCmdAttrib("!1SLI21")]
    public static ISCPPacket Tape2 { get { return new ISCPPacket("!1SLI21"); } }

    [ISCPCmdAttrib("!1SLI22")]
    public static ISCPPacket Phono { get { return new ISCPPacket("!1SLI22"); } }

    [ISCPCmdAttrib("!1SLI23")]
    public static ISCPPacket CD { get { return new ISCPPacket("!1SLI23"); } }

    [ISCPCmdAttrib("!1SLI24")]
    public static ISCPPacket FM { get { return new ISCPPacket("!1SLI24"); } }

    [ISCPCmdAttrib("!1SLI25")]
    public static ISCPPacket AM { get { return new ISCPPacket("!1SLI25"); } }

    [ISCPCmdAttrib("!1SLI26")]
    public static ISCPPacket Tuner { get { return new ISCPPacket("!1SLI26"); } }

    [ISCPCmdAttrib("!1SLI27")]
    public static ISCPPacket DLNA { get { return new ISCPPacket("!1SLI27"); } }

    [ISCPCmdAttrib("!1SLI28")]
    public static ISCPPacket InternetRadio { get { return new ISCPPacket("!1SLI28"); } }

    [ISCPCmdAttrib("!1SLI29")]
    public static ISCPPacket FrontUSB { get { return new ISCPPacket("!1SLI29"); } }

    [ISCPCmdAttrib("!1SLI2A")]
    public static ISCPPacket RearUSB { get { return new ISCPPacket("!1SLI2A"); } }

    [ISCPCmdAttrib("!1SLI2B")]
    public static ISCPPacket Network { get { return new ISCPPacket("!1SLI2B"); } }

    [ISCPCmdAttrib("!1SLIQSTN")]
    public static ISCPPacket Status { get { return new ISCPPacket("!1SLIQSTN"); } }
  }
}
