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

      reader = new StreamReader(@"Content/" + _settings["StartFile"] + ".txt");
    }

    public string Next() {
      var line = reader.ReadLine();
      //if the line is a command
      if (line[0] == '¶') {
        var split = line.Split(' ');
        ParseCommand();
        line = Next();
      }
      //if the line is a display line
      else {
        var split = line.Split('¶');
        //if the line is dialogue and the format is: [shortcut]¶[displayline]
        if (split.Length > 1) {
          if (_settings[split[0]] == null) {
            //todo: throw error
          }
          else {
            global.currentLineStack.Add("Character", _settings[split[0]]);
          }
        }
      }
      return line;
    }

    public void ParseCommand() {

    }
  }
}
