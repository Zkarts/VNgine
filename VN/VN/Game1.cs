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

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    SpriteFont font;
    Parser _parser;
    MouseState currentMouseState = Mouse.GetState(), prevMouseState = Mouse.GetState();
    string currentLine = "Start line";

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    protected override void Initialize() {
      base.Initialize();
      _parser = new Parser(this);
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
        currentLine = _parser.Next();
      }

      prevMouseState = currentMouseState;

      base.Update(gameTime);
    }

    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      base.Draw(gameTime);

      spriteBatch.Begin();
      spriteBatch.DrawString(font, currentLine, new Vector2(100, 100), Color.Black);
      spriteBatch.End();
    }
  }
}