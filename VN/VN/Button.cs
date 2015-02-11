using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VN {
  public enum ButtonFunction {
    StartGame = 0,
    Exit = 1,
    LoadGame = 2,
    CGGallery = 3,

  }

  public class Button {
    public Rectangle BoundingBox;
    public string Choice;
    public string Text;
    public ButtonFunction Function;
    public Texture2D Sprite;
  }
}
