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

    StreamReader _reader;
    readonly Dictionary<string,string> _settings;
    public Dictionary<string, string> CurrentLineStack = new Dictionary<string, string>();

    //todo: deze moet eigenlijk echt niet hier... in de Save misschien?
    public Dictionary<string, bool> MadeDecisions = new Dictionary<string, bool>();

    //todo: deze moet eigenlijk ook echt niet hier, maar in de InGame
    public List<Button> Options = new List<Button>();
    Texture2D button;
    
    Game1 global;

    const int OptionWidth = 550;

    public Parser(Game1 game) {
      global = game;

      button = global.Content.Load<Texture2D>("button");

      _settings = new Dictionary<string,string>();

      var doc = XDocument.Load(@"Content/Setup.xml");
      XElement xel = (XElement)doc.Root.FirstNode;

      //reads the entire settings.xml file and saves the data in a usable format
      while (true) {
        if (xel.HasElements) {
          if (xel.Name == "Character")
          {
            xel.Descendants();
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
      _reader = new StreamReader(@"Content/" + _settings["StartFile"] + ".txt", Encoding.UTF7);
    }

    //Reads the next line and fills the CurrentLineStack if needed, to pass the necessary actions on to the GameState
    public string Next() {
      var line = _reader.ReadLine();
      //while the line is a command
      while (line[0] == '¶') {
        ParseCommand(line.Substring(1));
        if (line == "¶end") {
          return "";
        }
        if (line.Substring(0, 7) == "¶choice") {
          return line;
        }
        line = _reader.ReadLine();
      }

      //The next line is a display line
      var nameSplit = line.Split('¶');
      //if the line is dialogue and the format is: <name abbreviation>¶<displayline>
      if (nameSplit.Length > 1) {
        if (nameSplit[0] == "" || _settings[nameSplit[0]] == null) {
          //todo: throw error
        }
        else {
          //Adds the speakers name to the CurrentLineStack
          CurrentLineStack.Add("Character", _settings[nameSplit[0]]);
          //It's a spoken line, so add quotation marks
          line = "\"" + nameSplit[1] + "\"";
        }
      }
      return WrapText(line, 650);
    }

    //Parses the different commands
    //todo? het verwerken moet eigenlijk ergens anders denk ik?
    public void ParseCommand(string line) {
      var split = line.Split(' ');
      switch (split[0]) {
        case "next":
          //if the command is in the format: next <decision> filename
          if (split.Length == 3) {
            if (MadeDecisions[split[1]]) {
              _reader = new StreamReader(@"Content/" + split.Last() + ".txt", Encoding.UTF7);
            }
          }
          else {
            _reader = new StreamReader(@"Content/" + split.Last() + ".txt", Encoding.UTF7);
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
            var option = _reader.ReadLine();
            string text = WrapText(option.Substring(option.IndexOf(' ') + 1), OptionWidth);
            int lines = text.Split('\n').Length;
            Options.Add(new Button {
              BoundingBox = new Rectangle(150, 100 + global.font.LineSpacing * (i + extraLines), OptionWidth, global.font.LineSpacing * lines),
              Choice = option.Split(' ')[0].Substring(1),
              Text = text,
              Sprite = button,
            });
            extraLines += lines - 1;
            MadeDecisions.Add(option.Split(' ')[0].Substring(1), false);
          }
          break;
        case "end":
          //Ends a game and clears the data
          MadeDecisions.Clear();
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
