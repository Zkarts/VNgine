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
      if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) {
        currentLine = _parser.Next();
      }

      if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released) {
        //Handle options
        if (_parser.options.Any()) {
          foreach (var option in _parser.options) {
            //if clicked on the option button
            if (option.BoundingBox.Contains(new Point(currentMouseState.X, currentMouseState.Y))) {
              //set the option to true in choices
              _parser.choices[option.Choice] = true;
              //clear the current options
              _parser.options.Clear();
              currentLine = _parser.Next();
              ProcessCurrentLineStack();
              displayString = "";
              break;
            }
          }
        }
        else {
          if (displayString == currentLine) {
            displayString = "";
            currentLine = _parser.Next();
            ProcessCurrentLineStack();
          }
          else {
            displayString = currentLine;
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
      if (_parser.options.Any()) {
        foreach (var option in _parser.options) {
          //todo: dit moet beter
          var button = global.Content.Load<Texture2D>("button");
          global.spriteBatch.Draw(button, option.BoundingBox, Color.White);
          //todo end
          global.spriteBatch.DrawString(global.font, option.Text, new Vector2(option.BoundingBox.Location.X, option.BoundingBox.Location.Y), Color.Black);
        }
      }
      else {
        global.spriteBatch.DrawString(global.font, displayString, new Vector2(100, 100), Color.Black);
        if (name != "") {
          global.spriteBatch.DrawString(global.font, name, new Vector2(100, 80), Color.Black);
        }
      }
    }

    public override void Reset() {

    }
  }
}