using System.Collections.Generic;

namespace HGJ.ConsoleLib {
  public class ConsoleRegion {
    public ConsolePoint Origin { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }

    public List<string> Content { get; set; }
  }
}
