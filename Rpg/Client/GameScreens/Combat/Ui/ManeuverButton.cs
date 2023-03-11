using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat.Ui
{
    internal class ManeuverButton : ButtonBase
    {
        public ManeuverButton(FieldCoords fieldCoords)
        {
            FieldCoords = fieldCoords;
        }

        public FieldCoords FieldCoords { get; }

        protected override Point CalcTextureOffset() => ControlTextures.Skill;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            
        }
    }
}
