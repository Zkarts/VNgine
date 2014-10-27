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
        ParseCommand(line);
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
          line = nameSplit[1];
        }
      }
      return line;
    }

    public void ParseCommand(string line) {
      var split = line.Substring(1).Split(' ');
      switch (split[0]) {
        case "next":
          reader = new StreamReader(@"Content/" + split.Last() + ".txt", Encoding.UTF7);
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
        case "check":
          break;
        default:
          break;
      }
    }
  }
}
