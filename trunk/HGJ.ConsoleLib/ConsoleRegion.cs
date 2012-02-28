﻿using System;
using System.Collections.Generic;

namespace HGJ.ConsoleLib {
  public delegate void ContentUpdated();

  public class ConsoleRegion {
    public ConsoleRegion(ConsolePoint origin, int height, int width, string title, bool visible) {
      Content = new List<string>();

      Origin = origin;
      Height = height;
      Width = width;
      Title = title;
      Visible = visible;
    }

    public event ContentUpdated OnContentUpdated;
    public ConsolePoint Origin { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool Visible { get; set; }

    public string Title { get; set; }

    public List<string> Content { get; set; }

    public void WriteContent(string message, bool append = false) {
      if (!append)
        Content = new List<string>();
      Content.AddRange(message.Split(new[] { "\r\n" }, StringSplitOptions.None));
      if (OnContentUpdated != null) OnContentUpdated();
    }

    public string GetInput(int lineNo, bool showInputChar = true) {
      Console.SetCursorPosition(Origin.X + 1, Origin.Y + lineNo);
      if (showInputChar)
        Console.Write("> ");
      return Console.ReadLine();
    }
    public ConsoleKeyInfo GetChar(int lineNo, bool showInputChar = true) {
      Console.SetCursorPosition(Origin.X + 1, Origin.Y + lineNo);
      if (showInputChar)
        Console.Write("> ");
      return Console.ReadKey();
    }
  }
}
