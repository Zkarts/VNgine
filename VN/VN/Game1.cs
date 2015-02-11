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
    public SpriteBatch spriteBatch;
    public SpriteFont font;

    bool finishedGame;
    GraphicsDeviceManager graphics;

    private GameState currentState;
    private Menu menu;
    private InGame inGame;

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    protected override void Initialize() {
      base.Initialize();
      IsMouseVisible = true;

      menu = new Menu(this);
      inGame = new InGame(this);
      currentState = menu;
    }

    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      font = Content.Load<SpriteFont>("font");
    }

    protected override void UnloadContent() {

    }

    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      base.Update(gameTime);
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      currentState.Update(gameTime);
    }

    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      base.Draw(gameTime);

      spriteBatch.Begin();

      currentState.Draw(gameTime);

      spriteBatch.End();
    }

    //Starts a new game
    public void StartGame() {
      inGame.StartGame();
      currentState = inGame;
    }

    //Ends a game and clears the data
    public void FinishGame() {
      currentState = menu;
      finishedGame = true;
    }
  }
}