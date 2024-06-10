using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombatDicesTeam.Engine.Ui;

public interface IUiElement
{
    Point Size { get; }

    void Draw(SpriteBatch spriteBatch);
}