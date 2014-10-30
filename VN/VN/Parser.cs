using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace VN {
  class Parser {
    StreamReader reader;
    Dictionary<string,string> _settings;
    Game1 global;

    public Parser(Game1 game) {
      global = game;
      _settings = new Dictionary<string,string>();

      var doc = XDocument.Load(@"Content/Setup.xml");
      XElement xel = (XElement)doc.Root.FirstNode;

      while (true) {
        if (xel.HasElements) {
          if (xel.Name == "Character") {
            var child = (XElement)xel.FirstNode;
            var childSibling = (XElement)xel.FirstNode.NextNode;
            _settings.Add(child.Value, childSibling.Value);
          }
        }
        else {
          if (xel.Name == "StartFile") {
            _settings.Add(xel.Name.LocalName, xel.Value);
          }
        }

        if (xel.NextNode != null) {
          xel = (XElement)xel.NextNode;
        }
        else {
          break;
        }
      }

      reader = new StreamReader(@"Content/" + _settings["StartFile"] + ".txt", Encoding.UTF7);
    }

    public string Next() {
      var line = reader.ReadLine();
      //if the line is a command
      while (line[0] == '¶') {
        ParseCommand(line.Substring(1));
        line = reader.ReadLine();
      }

      //if the line is a display line
      var nameSplit = line.Split('¶');
      //if the line is dialogue and the format is: [abbreviation]¶[displayline]
      if (nameSplit.Length > 1) {
        if (nameSplit[0] == "" || _settings[nameSplit[0]] == null) {
          //todo: throw error
        }
        else {
          global.currentLineStack.Add("Character", _settings[nameSplit[0]]);
          line = "\"" + nameSplit[1] + "\"";
        }
      }
      return line;
    }

    public void ParseCommand(string line) {
      var split = line.Split(' ');
      switch (split[0]) {
        case "next":
          if (split.Length == 3) {

          }
          else {
            reader = new StreamReader(@"Content/" + split.Last() + ".txt", Encoding.UTF7);
          }
          break;
        case "set":
          if (split[1] == "bg") {

          }
          else if (split[1] == "l1") {

          }
          else if (split[1] == "l2") {

          }
          else if (split[1] == "r1") {

          }
          else if (split[1] == "r2") {

          }
          break;
        case "play":
          if (split[1] == "music") {

          }
          else if (split[1] == "sound") {
          
          }
          else if (split[1] == "video") {

          }
          break;
        case "choice":
          for (int i = int.Parse(split[1]); i > 0; i--) {
            //parse options
            var option = reader.ReadLine();
            global.currentLineStack.Add("option|" + option.Split(' ')[0].Substring(1), option.Substring(option.IndexOf(' ') + 1));
          }
          line = split.Count() == 3 ? split[2] : "";
          break;
        case "end":
          break;
        default:
          break;
      }
    }
  }
}
