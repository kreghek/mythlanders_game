using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Upgrade
{
    internal sealed class UpgradeScreen : GameScreenBase
    {
        private readonly GlobeProvider _globeProvider;

        public UpgradeScreen(EwarGame game) : base(game)
        {
            _globeProvider = game.Services.GetService<GlobeProvider>();
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
