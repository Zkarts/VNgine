using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VN {
  public class InGame : GameState {
    KeyboardState currentKeyboardState, prevKeyboardState;
    bool autoMode;
    double autoTimer;
    Parser _parser;
    string currentLine = "Start line", displayString = "", name = "";

    public InGame(Game1 game) : base(game) {
      _parser = new Parser(global);
    }

    public override void Update(GameTime gameTime) {
      prevKeyboardState = currentKeyboardState;
      currentKeyboardState = Keyboard.GetState();
      prevMouseState = currentMouseState;
      currentMouseState = Mouse.GetState();
      HandleInput(currentMouseState, prevMouseState);

      //Display the text letter by letter
      if (displayString != currentLine) {
        displayString += currentLine[displayString.Length];
      }
      else {
        if (autoMode && !_parser.Options.Any()) {
          autoTimer -= gameTime.ElapsedGameTime.TotalSeconds;
          if (autoTimer <= 0) {
            NextLine();
          }
        }
      }
    }

    //Starts a new game
    public void StartGame() {
      autoMode = false;
      _parser.NewGame();
      NextLine();
    }

    public override void HandleInput(MouseState currentMouseState, MouseState prevMouseState) {
      if (currentKeyboardState.IsKeyDown(Keys.A) && !prevKeyboardState.IsKeyDown(Keys.A)) {
        autoMode = !autoMode;
      }

      if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released) {
        //Handle options
        if (!_parser.Options.Any()) {
          //if there are no clickable options
          if (displayString == currentLine) {
            NextLine();
          }
          else {
            displayString = currentLine;
          }
        }
        else {
          foreach (var option in _parser.Options) {
            //if clicked on the option button
            if (option.BoundingBox.Contains(new Point(currentMouseState.X, currentMouseState.Y))) {
              //set the option to true in choices
              _parser.MadeDecisions[option.Choice] = true;
              //clear the current options
              _parser.Options.Clear();
              currentLine = _parser.Next();
              ProcessCurrentLineStack();
              displayString = "";
              break;
            }
          }
        }
      }
    }

    private void NextLine() {
      displayString = "";
      currentLine = _parser.Next();
      autoTimer = Math.Max(currentLine.Length * 0.025, 1.5);
      ProcessCurrentLineStack();
    }

    //Processes everything in the CurrentLineStack
    private void ProcessCurrentLineStack() {
      if (_parser.CurrentLineStack.ContainsKey("Character")) {
        name = _parser.CurrentLineStack["Character"];
        _parser.CurrentLineStack.Remove("Character");
      }
      else {
        name = "";
      }
    }

    public override void Draw(GameTime gameTime) {
      if (!_parser.Options.Any()) {
        //if there are no clickable options; draw the text and possible name
        global.spriteBatch.DrawString(global.font, displayString, new Vector2(100, 100), Color.Black);
        if (name != "") {
          global.spriteBatch.DrawString(global.font, name, new Vector2(100, 80), Color.Black);
        }
        if (autoMode) {
          global.spriteBatch.DrawString(global.font, "Auto " + autoTimer, new Vector2(600, 300), Color.DarkGreen);
        }
      }
      else {
        foreach (var option in _parser.Options) {
          global.spriteBatch.Draw(option.Sprite, option.BoundingBox, Color.White);
          global.spriteBatch.DrawString(global.font, option.Text, new Vector2(option.BoundingBox.Location.X + 15, option.BoundingBox.Location.Y), Color.Black);
        }
      }
    }

    public override void Reset() {

    }
  }
}