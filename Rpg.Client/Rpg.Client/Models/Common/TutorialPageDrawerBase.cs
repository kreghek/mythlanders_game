using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Models.Common
{
    internal abstract class TutorialPageDrawerBase
    {
        public abstract void Draw(SpriteBatch spriteBatch, Rectangle contentRect);
    }

    internal class CombatTutorialPageDrawer : TutorialPageDrawerBase
    {
        public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            
        }
    }
}
