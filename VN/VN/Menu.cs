using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VN {
  public class Menu {
    Texture2D button;

    List<Button> current;
    List<Button> MainMenu;
    List<Button> Settings;
    List<Button> Save;
    List<Button> Load;
    Game1 global;

    public Menu(Game1 game) {
      global = game;
      InitializeMenus();
      current = MainMenu;
    }

    void InitializeMenus() {
      button = global.Content.Load<Texture2D>("button");
      MainMenu = new List<Button> {
        new Button {
          BoundingBox = new Rectangle(100, 100, 200, 40),
          Text = "Start game",
          Sprite = button,
        },
        new Button {
          BoundingBox = new Rectangle(100, 150, 200, 40),
          Text = "Load game",
          Sprite = button,
        },
        new Button {
          BoundingBox = new Rectangle(100, 200, 200, 40),
          Text = "CG Gallery",
          Sprite = button,
        },
        new Button {
          BoundingBox = new Rectangle(100, 250, 200, 40),
          Text = "Exit",
          Sprite = button,
        },
      };
    }

    //Handles clicking the buttons for the current menu
    public void HandleMenu(MouseState currentMouseState, MouseState prevMouseState) {
      if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released) {
        foreach (var button in current) {
          if (button.BoundingBox.Contains(new Point(currentMouseState.X, currentMouseState.Y))) {
            switch (button.Text) {
              case "Start game":
                global.StartGame();
                break;
              case "Exit":
                global.Exit();
                break;
              default:
                break;
            }
            break;
          }
        }
      }
    }

    //Draws the buttons in the current menu
    public void Draw(GameTime gameTime) {
      foreach (Button b in current) {
        DrawMenuButton(b, gameTime);
      }
    }

    //Draws the actual buttons
    void DrawMenuButton(Button button, GameTime gameTime) {
      if (button.Sprite != null) {
        global.spriteBatch.Draw(button.Sprite, button.BoundingBox, Color.White);
      }
      global.spriteBatch.DrawString(global.font, button.Text, new Vector2(button.BoundingBox.X, button.BoundingBox.Y), Color.Black);
    }
  }
}
