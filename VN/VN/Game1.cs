using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VN {
  public class Game1 : Microsoft.Xna.Framework.Game {
    public Dictionary<string, string> currentLineStack = new Dictionary<string, string>();
    public Dictionary<string, bool> choices = new Dictionary<string,bool>();
    public List<Button> options = new List<Button>();

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    SpriteFont font;
    Parser _parser;
    MouseState currentMouseState = Mouse.GetState(), prevMouseState = Mouse.GetState();
    string currentLine = "Start line", name = "";

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    protected override void Initialize() {
      base.Initialize();
      _parser = new Parser(this);
      IsMouseVisible = true;
    }

    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      font = Content.Load<SpriteFont>("font");

    }

    protected override void UnloadContent() {

    }

    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();
      if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) {
        currentLine = _parser.Next();
      }

      currentMouseState = Mouse.GetState();

      if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released) {
        //Handle options
        if (options.Any()) {
          foreach (var option in options) {
            if (option.BoundingBox.Contains(new Point(currentMouseState.X, currentMouseState.Y))) {
              choices[option.Choice] = true;
              options.Clear();
              currentLine = _parser.Next();
              ProcessCurrentLineStack();
              break;
            }
          }
        }
        else {
          currentLine = _parser.Next();
          ProcessCurrentLineStack();
        }
      }

      prevMouseState = currentMouseState;

      base.Update(gameTime);
    }

    private void ProcessCurrentLineStack() {
      if (currentLineStack.ContainsKey("Character")) {
        name = currentLineStack["Character"];
        currentLineStack.Remove("Character");
      }
      else {
        name = "";
      }
    }

    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      base.Draw(gameTime);

      spriteBatch.Begin();
      if (options.Any()) {
        foreach (var option in options) {
          spriteBatch.DrawString(font, option.Text, new Vector2(option.BoundingBox.Location.X, option.BoundingBox.Location.Y), Color.Black);
        }
      }
      else {
        spriteBatch.DrawString(font, currentLine, new Vector2(100, 100), Color.Black);
        if (name != "") {
          spriteBatch.DrawString(font, name, new Vector2(100, 80), Color.Black);
        }
      }
      spriteBatch.End();
    }
  }
}