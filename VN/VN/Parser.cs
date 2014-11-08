﻿using System;
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

    }

    public void NewGame() {
      reader = new StreamReader(@"Content/" + _settings["StartFile"] + ".txt", Encoding.UTF7);
    }

    public string Next() {
      var line = reader.ReadLine();
      //if the line is a command
      while (line[0] == '¶') {
        ParseCommand(line.Substring(1));
        if (global.state != Game1.GameState.InGame) {
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
          global.currentLineStack.Add("Character", _settings[nameSplit[0]]);
          line = "\"" + nameSplit[1] + "\"";
        }
      }
      return WrapText(line, 650);
    }

    public void ParseCommand(string line) {
      var split = line.Split(' ');
      switch (split[0]) {
        case "next":
          if (split.Length == 3) {
            if (global.choices[split[1]]) {
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
          int optionWidth = 550;
          int extra = 0;
          for (int i = 0; i < int.Parse(split[1]); i++) {
            //parse options
            var option = reader.ReadLine();
            string text = WrapText(option.Substring(option.IndexOf(' ') + 1), optionWidth);
            int lines = text.Split('\n').Length;
            global.options.Add(new Button { BoundingBox = new Rectangle(150, 100 + global.font.LineSpacing * (i + extra), optionWidth, global.font.LineSpacing * lines), Choice = option.Split(' ')[0].Substring(1), Text = text });
            extra += lines - 1;
            global.choices.Add(option.Split(' ')[0].Substring(1), false);
          }
          break;
        case "end":
          global.FinishGame();
          break;
        default:
          break;
      }
    }

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
