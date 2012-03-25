using System;

namespace OnkyoISCPlib {
  internal class ISCPCmdAttrib : Attribute {
    public ISCPCmdAttrib(string name) {
      Name = name;
    }

    //public ISCPCmdAttrib(string name, string regex) {
    //  Name = name;
    //  Regex = regex;
    //}
    public ISCPCmdAttrib(string name, string displayName) {
      Name = name;
      DisplayName = displayName;
    }

    public string Name { get; set; }
    public string Regex { get; set; }
    public string DisplayName { get; set; }
  }
}