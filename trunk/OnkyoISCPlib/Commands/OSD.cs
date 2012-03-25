using System;

namespace OnkyoISCPlib.Commands {
  public class OSD : ISCPPacket {
    /*"OSD" - Setup Operation Command 	
      "MENU"	Menu Key
      "UP"	Up Key
      "DOWN"	Down Key
      "RIGHT"	Right Key
      "LEFT"	Left Key
      "ENTER"	Enter Key
      "EXIT"	Exit Key
      "AUDIO"	Audio Adjust Key
      "VIDEO"	Video Adjust Key
      "HOME"	Home Key
      */

    public OSD(string command, string name)
      : base(command) {
      Name = name;
    }

    public static OSD Menu {
      get { return new OSD("!1OSDMENU", "Menu"); }
    }

    public static OSD Up {
      get { return new OSD("!1OSDUP", "Up"); }
    }

    public static OSD Down {
      get { return new OSD("!1OSDDOWN", "Down"); }
    }

    public static OSD Right {
      get { return new OSD("!1OSDRIGHT", "Right"); }
    }

    public static OSD Left {
      get { return new OSD("!1OSDLEFT", "Left"); }
    }

    public static OSD Enter {
      get { return new OSD("!1OSDENTER", "Enter"); }
    }

    public static OSD Exit {
      get { return new OSD("!1OSDEXIT", "Exit"); }
    }

    public static OSD Audio {
      get { return new OSD("!1OSDAUDIO", "Audio"); }
    }

    public static OSD Video {
      get { return new OSD("!1OSDVIDEO", "Video"); }
    }

    public static OSD Home {
      get { return new OSD("!1OSDHOME", "Home"); }
    }

    public string Name { get; set; }

    public static new OSD ParsePacket(string command) {
      switch (command) {
        case "MENU":
          return Menu;
        case "UP":
          return Up;
        case "DOWN":
          return Down;
        case "RIGHT":
          return Right;
        case "LEFT":
          return Left;
        case "ENTER":
          return Enter;
        case "EXIT":
          return Exit;
        case "AUDIO":
          return Audio;
        case "VIDEO":
          return Video;
        case "HOME":
          return Home;
      }
      throw new ArgumentException("Cannot find the command", "command");
    }

    public override string ToString() {
      return string.Format("OSD Command: {0}" + Name);
    }
  }
}