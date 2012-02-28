using System;
using System.Collections.Generic;

namespace HGJ.ConsoleLib {
  public class HGJConsole {
    public ConsoleColor BackgroundColor { get; set; }
    public List<ConsoleRegion> Regions { get; set; }

    public HGJConsole() {
      Regions = new List<ConsoleRegion>();
      BackgroundColor = ConsoleColor.Black;
    }

    public void Draw() {
      Console.BackgroundColor = BackgroundColor;
      Console.Clear();

      foreach (var consoleRegion in Regions) {
        Console.SetCursorPosition(consoleRegion.Origin.X, consoleRegion.Origin.Y);
        //check for height...
        foreach (string line in consoleRegion.Content) {
          Console.WriteLine(line.Length > consoleRegion.Width ? line.Substring(0, consoleRegion.Width) : line);
          Console.SetCursorPosition(consoleRegion.Origin.X, consoleRegion.Origin.Y + 1);
        }
      }
    }
  }
}
