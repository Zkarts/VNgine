using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VN {
  public class InGame : GameState {
    Parser _parser;
    string currentLine = "Start line", displayString = "", name = "";

    public InGame(Game1 game) : base(game) {
      _parser = new Parser(global);
    }

    public override void Update(GameTime gameTime) {
      prevMouseState = currentMouseState;
      currentMouseState = Mouse.GetState();
      HandleInput(currentMouseState, prevMouseState);

      //Display the text letter by letter
      if (displayString != currentLine) {
        displayString += currentLine[displayString.Length];
      }
    }

    //Starts a new game
    public void StartGame() {
      _parser.NewGame();
      currentLine = _parser.Next();
    }

    public override void HandleInput(MouseState currentMouseState, MouseState prevMouseState) {
      if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released) {
        //Handle options
        if (!_parser.options.Any()) {
          //if there are no clickable options
          if (displayString == currentLine) {
            displayString = "";
            currentLine = _parser.Next();
            ProcessCurrentLineStack();
          }
          else {
            displayString = currentLine;
          }
        }
        else {
          foreach (var option in _parser.options) {
            //if clicked on the option button
            if (option.BoundingBox.Contains(new Point(currentMouseState.X, currentMouseState.Y))) {
              //set the option to true in choices
              _parser.madeDecisions[option.Choice] = true;
              //clear the current options
              _parser.options.Clear();
              currentLine = _parser.Next();
              ProcessCurrentLineStack();
              displayString = "";
              break;
            }
          }
        }
      }
    }

    //Processes everything in the CurrentLineStack
    private void ProcessCurrentLineStack() {
      if (_parser.currentLineStack.ContainsKey("Character")) {
        name = _parser.currentLineStack["Character"];
        _parser.currentLineStack.Remove("Character");
      }
      else {
        name = "";
      }
    }

    public override void Draw(GameTime gameTime) {
      if (!_parser.options.Any()) {
        //if there are no clickable options; draw the text and possible name
        global.spriteBatch.DrawString(global.font, displayString, new Vector2(100, 100), Color.Black);
        if (name != "") {
          global.spriteBatch.DrawString(global.font, name, new Vector2(100, 80), Color.Black);
        }
      }
      else {
        foreach (var option in _parser.options) {
          global.spriteBatch.Draw(option.Sprite, option.BoundingBox, Color.White);
          global.spriteBatch.DrawString(global.font, option.Text, new Vector2(option.BoundingBox.Location.X, option.BoundingBox.Location.Y), Color.Black);
        }
      }
    }

    public override void Reset() {

    }
  }
}