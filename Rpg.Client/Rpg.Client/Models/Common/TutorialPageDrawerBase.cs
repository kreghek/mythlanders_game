using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Common
{
    internal abstract class TutorialPageDrawerBase
    {
        protected TutorialPageDrawerBase(IUiContentStorage uiContentStorage)
        {
            UiContentStorage = uiContentStorage;
        }

        internal IUiContentStorage UiContentStorage { get; }

        public abstract void Draw(SpriteBatch spriteBatch, Rectangle contentRect);
    }
}