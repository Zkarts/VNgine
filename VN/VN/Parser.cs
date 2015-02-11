using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VN {
  class Parser {
    StreamReader reader;
    Dictionary<string,string> _settings;
    public Dictionary<string, string> currentLineStack = new Dictionary<string, string>();
    public Dictionary<string, bool> choices = new Dictionary<string, bool>();
    public List<Button> options = new List<Button>();
    Game1 global;

    const int OptionWidth = 550;

    public Parser(Game1 game) {
      global = game;
      _settings = new Dictionary<string,string>();

      var doc = XDocument.Load(@"Content/Setup.xml");
      XElement xel = (XElement)doc.Root.FirstNode;

      //reads the entire settings.xml file and saves the data in a usable format
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

    }

    //Refreshes the StreamReader for a new game
    public void NewGame() {
      reader = new StreamReader(@"Content/" + _settings["StartFile"] + ".txt", Encoding.UTF7);
    }

    //Reads the next line and fills the CurrentLineStack if needed
    public string Next() {
      var line = reader.ReadLine();
      //if the line is a command
      while (line[0] == '¶') {
        ParseCommand(line.Substring(1));
        if (line == "¶end") {
          return "";
        }
        if (line.Substring(0, 7) == "¶choice") {
          return line;
        }
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
          //Adds the speakers name to the CurrentLineStack
          currentLineStack.Add("Character", _settings[nameSplit[0]]);
          line = "\"" + nameSplit[1] + "\"";
        }
      }
      return WrapText(line, 650);
    }

    //Parses the different commands
    public void ParseCommand(string line) {
      var split = line.Split(' ');
      switch (split[0]) {
        case "next":
          //if the command is in the format: next [boolean choice] filename
          if (split.Length == 3) {
            if (choices[split[1]]) {
              reader = new StreamReader(@"Content/" + split.Last() + ".txt", Encoding.UTF7);
            }
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
          int extraLines = 0;
          for (int i = 0; i < int.Parse(split[1]); i++) {
            //parse options
            var option = reader.ReadLine();
            string text = WrapText(option.Substring(option.IndexOf(' ') + 1), OptionWidth);
            int lines = text.Split('\n').Length;
            options.Add(new Button {
              BoundingBox = new Rectangle(150, 100 + global.font.LineSpacing * (i + extraLines), OptionWidth, global.font.LineSpacing * lines),
              Choice = option.Split(' ')[0].Substring(1),
              Text = text
            });
            extraLines += lines - 1;
            choices.Add(option.Split(' ')[0].Substring(1), false);
          }
          break;
        case "end":
          //Ends a game and clears the data
          choices.Clear();
          global.FinishGame();
          break;
        default:
          break;
      }
    }

    //Wraps text within a box of the width and returns the wrapped string
    private String WrapText(String text, int width) {
      String line = String.Empty;
      String returnString = String.Empty;
      String[] wordArray = text.Split(' ');

      foreach (String word in wordArray) {
        if (global.font.MeasureString(line + word).Length() > width) {
          returnString = returnString + line + '\n';
          line = String.Empty;
        }
        line = line + word + ' ';
      }

      return returnString + line;
    }
  }
}
