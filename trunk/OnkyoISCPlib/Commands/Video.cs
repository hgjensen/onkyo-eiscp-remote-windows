namespace OnkyoISCPlib.Commands {
  public class Audio : ISCPPacket {
    /*"IFA" - Audio Infomation Command	
    "nnnnn:nnnnn"	Infomation of Audio(Same Immediate Display ',' is separator of infomations)
    "QSTN"	gets Infomation of Audio
    when you send "DIF02", receiver will return this response.	
    */

    private Audio()
      : base("!1IFAQSTN") {
    }

    public Audio(string command, string name)
      : base(command) {
      Name = name;
    }

    public static Audio Status {
      get { return new Audio("!1IFAQSTN", "Status"); }
    }

    public string Name { get; set; }

    public string InputSource { get; set; }
    public string InputFormat { get; set; }
    public string InputFreq { get; set; }
    public string InputChannels { get; set; }

    public string OutputMode { get; set; }
    public string OutputChannels { get; set; }

    public new static Audio ParsePacket(string command) {
      string[] parts = command.Split(',');
      return new Audio {
        InputSource = parts[0],
        InputFormat = parts[1],
        InputFreq = parts[2],
        InputChannels = parts[3],
        OutputMode = parts[4],
        OutputChannels = parts[5]
      };
    }

    public override string ToString() {
      return string.Format("Input: {0}, {1}, {2}, {3}\r\nOutput: {4}, {5}",
                           InputSource, InputFormat, InputFreq, InputChannels, OutputMode, OutputChannels);
    }
  }
}