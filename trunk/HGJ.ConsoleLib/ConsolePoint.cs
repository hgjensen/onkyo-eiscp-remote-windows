using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HGJ.ConsoleLib {
  public class ConsolePoint {
    public ConsolePoint(int left, int top) {
      X = left;
      Y = top;
    }
    public int X { get; set; } //left
    public int Y { get; set; } //top
  }
}
