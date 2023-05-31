using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal interface ICombatantThumbnailProvider
{
    Texture2D Get(string classSid);
}
