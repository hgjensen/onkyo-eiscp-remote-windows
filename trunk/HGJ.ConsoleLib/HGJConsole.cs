using System;
using System.Collections.Generic;
using System.Linq;

namespace HGJ.ConsoleLib {
  public static class HGJConsole {
    private const string corner = "+";
    private const string hori = "-";
    private const string vert = "|";
    private const string blank = " ";

    public static ConsoleColor BackgroundColor {
      get {
        if (_backgroundColor == default(ConsoleColor))
          _backgroundColor = ConsoleColor.Black;
        return _backgroundColor;
      }
      set {
        _backgroundColor = value;
      }
    }
    public static RegionDictionary<string, ConsoleRegion> Regions {
      get {
        if (_regions == default(RegionDictionary<string, ConsoleRegion>)) {
          _regions = new RegionDictionary<string, ConsoleRegion>();
          _regions.AddEvent += Regions_AddEvent;
          _regions.RemoveEvent += Regions_RemoveEvent;
        }
        return _regions;
      }
      set {
        _regions = value;
      }
    }
    public static int ConsoleHeight { get { return Console.WindowHeight; } }
    public static int ConsoleWidth { get { return Console.WindowWidth; } }

    private static ConsoleColor _backgroundColor;
    private static RegionDictionary<string, ConsoleRegion> _regions;

    static void Regions_RemoveEvent(RegionDictionary<string, ConsoleRegion>.RemoveEventArgs pRemoveEventArgs) {
      Draw();
    }
    static void Regions_AddEvent(RegionDictionary<string, ConsoleRegion>.AddEventArgs pAddEventArgs) {
      pAddEventArgs.Value.OnContentUpdated += Value_OnContentUpdated;
      Draw();
    }
    static void Value_OnContentUpdated() {
      Draw();
    }

    public static void Draw(bool clear = false) {
      if (clear) {
        Console.BackgroundColor = BackgroundColor;
        Console.Clear();
      }

      foreach (var consoleRegion in Regions.Where(p => p.Value.Visible)) {
        drawRegionBorder(consoleRegion.Value);
        Console.SetCursorPosition(consoleRegion.Value.Origin.X + 1, consoleRegion.Value.Origin.Y + 1);
        int h = 1;
        foreach (string line in consoleRegion.Value.Content) {
          string l = (line.Length > consoleRegion.Value.Width - 2
                               ? line.Substring(0, consoleRegion.Value.Width - 2)
                               : line);
          Console.WriteLine(l);
          Console.SetCursorPosition(consoleRegion.Value.Origin.X + 1, consoleRegion.Value.Origin.Y + 1 + h);
          h++;
          if (h >= consoleRegion.Value.Height)
            break;
        }
      }

      Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
    }

    private static void drawRegionBorder(ConsoleRegion r) {
      //bool touchLeft = Regions.Any(p => (p.Value.Origin.X + p.Value.Width) == r.Origin.X);
      //bool touchRight = Regions.Any(p => (p.Value.Origin.X) == r.Origin.X + r.Width);
      //bool touchTop = Regions.Any(p => (p.Value.Origin.Y + p.Value.Height) == r.Origin.Y);
      //bool touchBottom = Regions.Any(p => (p.Value.Origin.Y) == r.Origin.Y + r.Height);

      //Generate first line
      string firstline = corner;
      int lentitle = r.Title.Length;
      int w = r.Width;
      int titlestart = (w / 2) - (lentitle / 2);

      //Replace part of line with title
      for (int x = 0; x < r.Width; x++)
        if (x == titlestart) {
          firstline += r.Title;
          x += lentitle - 1;
        } else
          firstline += hori;
      firstline += corner;

      //Generate last line
      string lastline = corner;
      for (int x = 0; x < r.Width; x++)
        lastline += hori;
      lastline += corner;

      //Make space-buffer
      string space = string.Empty;
      for (int x = 0; x < r.Width; x++)
        space += blank;

      //Write border
      Console.SetCursorPosition(r.Origin.X, r.Origin.Y);
      Console.WriteLine(firstline);

      for (int x = 1; x <= r.Height; x++) {
        Console.SetCursorPosition(r.Origin.X, r.Origin.Y + x);
        Console.Write(vert + space + vert);
      }

      Console.SetCursorPosition(r.Origin.X, r.Origin.Y + r.Height);
      Console.WriteLine(lastline);
    }

    public static void Reset() {
      _regions = null;
      _backgroundColor = ConsoleColor.Black;
      Draw();
    }
  }
}
