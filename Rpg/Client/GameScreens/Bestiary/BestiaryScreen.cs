using System.Collections.Generic;
using System.Linq;

using Client;
using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Bestiary
{
    internal sealed class BestiaryScreen : GameScreenWithMenuBase
    {
        private readonly IList<ButtonBase> _buttonList;

        private readonly Player _player;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        private UnitScheme? _selectedMonster;

        public BestiaryScreen(TestamentGame game)
            : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            var globeProvider = game.Services.GetService<GlobeProvider>();
            _player = globeProvider.Globe.Player;

            _buttonList = new List<ButtonBase>();
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle));

            backButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Map, null);
            };

            return new[] { backButton };
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            ResolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            for (var index = 0; index < _buttonList.Count; index++)
            {
                var button = _buttonList[index];
                button.Rect = new Rectangle(contentRect.Left, contentRect.Top + index * 21, 100, 20);
                button.Draw(spriteBatch);
            }

            if (_selectedMonster is not null)
            {
                var sb = CollectMonsterStats(_selectedMonster, monsterLevel: 1);

                for (var statIndex = 0; statIndex < sb.Count; statIndex++)
                {
                    var line = sb[statIndex];
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), line,
                        new Vector2(contentRect.Center.X, contentRect.Top + statIndex * 22), Color.White);
                }
            }

            spriteBatch.End();
        }

        protected override void InitializeContent()
        {
            InitializeMonsterButtons();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            foreach (var button in _buttonList)
            {
                button.Update(ResolutionIndependentRenderer);
            }
        }

        private static IList<string> CollectMonsterStats(UnitScheme monsterScheme, int monsterLevel)
        {
            var monster = new Core.Hero(monsterScheme, monsterLevel);

            var unitName = monsterScheme.Name;
            var name = GameObjectHelper.GetLocalized(unitName);

            var sb = new List<string>
            {
                name,
                string.Format(UiResource.HitPointsLabelTemplate,
                    monster.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value.ActualMax),
                string.Format(UiResource.ShieldPointsLabelTemplate,
                    monster.Stats.Single(x => x.Type == UnitStatType.ShieldPoints).Value.ActualMax),
                string.Format(UiResource.DamageLabelTemplate, monster.Damage),
                string.Format(UiResource.ArmorLabelTemplate, monster.Armor)
            };

            foreach (var perk in monster.Perks)
            {
                var localizedName = GameObjectHelper.GetLocalized(perk);
                sb.Add(localizedName);

                var localizedDescription = GameObjectHelper.GetLocalizedDescription(perk);

                sb.Add(localizedDescription);
            }

            foreach (var skill in monster.Skills)
            {
                var localizedName = GameObjectHelper.GetLocalized(skill.Sid);

                sb.Add(localizedName);
            }

            return sb;
        }

        private void InitializeMonsterButtons()
        {
            var monsterSchemes = _unitSchemeCatalog.AllMonsters;

            _buttonList.Clear();

            foreach (var monsterScheme in monsterSchemes)
            {
                if (!_player.KnownMonsters.Contains(monsterScheme))
                {
                    // This monster is unknown. So do not display it on the screen.
                    continue;
                }

                var unitName = monsterScheme.Name;
                var name = GameObjectHelper.GetLocalized(unitName);

                var button = new TextButton(name);
                button.OnClick += (_, _) => { _selectedMonster = monsterScheme; };
                _buttonList.Add(button);
            }
        }
    }
}