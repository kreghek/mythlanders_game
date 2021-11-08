using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal sealed class UnitPanelController
    {
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly ActiveCombat _activeCombat;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public UnitPanelController(
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            ActiveCombat activeCombat,
            IUiContentStorage uiContentStorage,
            GameObjectContentStorage gameObjectContentStorage)
        {
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _activeCombat = activeCombat;
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var unitList = _activeCombat.Units.ToArray();

            var playerIndex = 0;
            var monsterIndex = 0;

            foreach (var combatUnit in unitList)
            {
                Rectangle panelPosition;

                if (combatUnit.Unit.IsPlayerControlled)
                {
                    panelPosition = new Rectangle(0, playerIndex * 48, 128, 48);
                    playerIndex++;
                }
                else
                {
                    panelPosition = new Rectangle(_resolutionIndependentRenderer.VirtualWidth - 128, monsterIndex * 48, 128, 48);
                    monsterIndex++;
                }

                spriteBatch.Draw(_uiContentStorage.GetUnitPanelTexture(), panelPosition, new Rectangle(0, 0, 128, 48),
                    Color.White);

                var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(combatUnit.Unit.UnitScheme.Name);
                var portraitPosition = panelPosition.Location.ToVector2() + new Vector2(7, 0);
                var portraitDestRect = new Rectangle(portraitPosition.ToPoint(), new Point(32, 32));
                spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(), portraitDestRect, portraitSourceRect,
                    Color.White);

                var hpPosition = panelPosition.Location.ToVector2() + new Vector2(55, 20);
                var hpDestRect = new Rectangle(hpPosition.ToPoint(), new Point(70, 10));
                var hpPercentage = (float)combatUnit.Unit.Hp / combatUnit.Unit.MaxHp;
                var hpSourceRect = new Rectangle(0, 50, (int)(hpPercentage * 128), 10);
                spriteBatch.Draw(_uiContentStorage.GetUnitPanelTexture(), hpPosition, hpSourceRect,
                    Color.Lerp(Color.Transparent, Color.White, 0.75f));

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{combatUnit.Unit.Hp}/{combatUnit.Unit.MaxHp}",
                    hpPosition, Color.Black);
            }
        }
    }
}
