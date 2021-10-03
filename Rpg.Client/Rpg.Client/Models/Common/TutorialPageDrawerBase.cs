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
        private readonly IUiContentStorage _uiContentStorage;

        protected TutorialPageDrawerBase(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
        }

        internal IUiContentStorage UiContentStorage => _uiContentStorage;

        public abstract void Draw(SpriteBatch spriteBatch, Rectangle contentRect);
    }
}