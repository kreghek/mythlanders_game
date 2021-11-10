using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal abstract class HintBase: ControlBase
    {
        protected HintBase(Texture2D texture): base(texture)
        {
        }

        protected override Color CalculateColor()
        {
            return Color.Lerp(Color.Transparent, Color.SlateBlue, 0.75f);
        }
    }
}