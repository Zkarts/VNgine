using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VN {
  public abstract class GameState {
    protected Game1 global;
    protected MouseState currentMouseState, prevMouseState;

    public GameState(Game1 game) {
      global = game;
    }

    public abstract void Update(GameTime gameTime);

    public abstract void HandleInput(MouseState currentMouseState, MouseState prevMouseState);

    public abstract void Draw(GameTime gameTime);

    public abstract void Reset();
  }
}