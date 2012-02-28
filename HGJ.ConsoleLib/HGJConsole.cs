using System;
using System.Linq;

namespace HGJ.ConsoleLib {
  public class HGJConsole {
    public ConsoleColor BackgroundColor { get; set; }
    public RegionDictionary<string, ConsoleRegion> Regions { get; set; }
    public int ConsoleHeight { get { return Console.WindowHeight; } }
    public int ConsoleWidth { get { return Console.WindowWidth; } }

    public HGJConsole() {
      Regions = new RegionDictionary<string, ConsoleRegion>();
      Regions.AddEvent += Regions_AddEvent;
      Regions.RemoveEvent += Regions_RemoveEvent;
      BackgroundColor = ConsoleColor.Black;
    }

    void Regions_RemoveEvent(RegionDictionary<string, ConsoleRegion>.RemoveEventArgs pRemoveEventArgs) {
      Draw();
    }
    void Regions_AddEvent(RegionDictionary<string, ConsoleRegion>.AddEventArgs pAddEventArgs) {
      pAddEventArgs.Value.OnContentUpdated += Value_OnContentUpdated;
      Draw();
    }
    void Value_OnContentUpdated() {
      Draw();
    }

    public void Draw() {
      Console.BackgroundColor = BackgroundColor;
      Console.Clear();

      foreach (var consoleRegion in Regions.Where(p => p.Value.Visible)) {
        drawRegionBorder(consoleRegion.Value);
        Console.SetCursorPosition(consoleRegion.Value.Origin.X + 1, consoleRegion.Value.Origin.Y + 1);
        int h = 1;
        int w = consoleRegion.Value.Width;

        foreach (string line in consoleRegion.Value.Content) {
          Console.WriteLine((line.Length > consoleRegion.Value.Width - 2
                               ? line.Substring(0, consoleRegion.Value.Width - 2)
                               : line));
          //Console.WriteLine(line.Length > consoleRegion.Value.Width ? line.Substring(0, consoleRegion.Value.Width) : line);
          Console.SetCursorPosition(consoleRegion.Value.Origin.X + 1, consoleRegion.Value.Origin.Y + 1 + h);
          h++;
          if (h >= consoleRegion.Value.Height)
            break;
        }
      }

      Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
    }

    private void drawRegionBorder(ConsoleRegion r) {
      bool touchLeft = Regions.Any(p => (p.Value.Origin.X + p.Value.Width) == r.Origin.X);
      bool touchRight = Regions.Any(p => (p.Value.Origin.X) == r.Origin.X + r.Width);
      bool touchTop = Regions.Any(p => (p.Value.Origin.Y + p.Value.Height) == r.Origin.Y);
      bool touchBottom = Regions.Any(p => (p.Value.Origin.Y) == r.Origin.Y + r.Height);

      string firstline = "/";

      int lentitle = r.Title.Length;
      int w = r.Width;
      int titlestart = (w / 2) - (lentitle / 2);

      for (int x = 0; x < r.Width; x++)
        if (x == titlestart) {
          firstline += r.Title;
          x += lentitle - 1;
        } else
          firstline += "-";
      firstline += @"\";

      string lastline = @"\";
      for (int x = 0; x < r.Width; x++)
        lastline += "-";
      lastline += "/";

      string space = "";
      for (int x = 0; x < r.Width; x++)
        space += " ";

      Console.SetCursorPosition(r.Origin.X, r.Origin.Y);
      Console.WriteLine(firstline);

      for (int x = 1; x <= r.Height; x++) {
        Console.SetCursorPosition(r.Origin.X, r.Origin.Y + x);
        Console.Write("|" + space + "|");
      }

      Console.SetCursorPosition(r.Origin.X, r.Origin.Y + r.Height);
      Console.WriteLine(lastline);
    }
  }
}
