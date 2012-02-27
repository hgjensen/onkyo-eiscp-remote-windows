using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnkyoISCPlib {
  internal class ISCPCmdAttrib : Attribute {
    public string Name { get; set; }
    public string Regex { get; set; }

    public ISCPCmdAttrib(string name) {
      Name = name;
    }
    public ISCPCmdAttrib(string name, string regex) {
      Regex = regex;
    }
  }
}
