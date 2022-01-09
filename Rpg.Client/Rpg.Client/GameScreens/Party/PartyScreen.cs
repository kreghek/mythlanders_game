using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Party
{
    internal sealed class PartyScreen : GameScreenBase
    {
        private readonly Camera2D _camera;
        private readonly GlobeProvider _globeProvider;
        private readonly IList<ButtonBase> _navigationButtonList;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IList<ButtonBase> _slotButtonList;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IList<ButtonBase> _unitButtonList;

        private bool _isInitialized;
        private Unit? _selectedCharacter;

        public PartyScreen(EwarGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _globeProvider = game.Services.GetService<GlobeProvider>();

            _camera = game.Services.GetService<Camera2D>();
            _resolutionIndependentRenderer = game.Services.GetService<ResolutionIndependentRenderer>();

            _navigationButtonList = new List<ButtonBase>();
            _unitButtonList = new List<ButtonBase>();
            _slotButtonList = new List<ButtonBase>();
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _resolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            var contentRect = _resolutionIndependentRenderer.VirtualBounds;

            for (var buttonIndex = 0; buttonIndex < _navigationButtonList.Count; buttonIndex++)
            {
                var button = _navigationButtonList[buttonIndex];
                button.Rect = new Rectangle(contentRect.Left, contentRect.Top + buttonIndex * 21, 100, 20);
                button.Draw(spriteBatch);
            }

            for (var buttonIndex = 0; buttonIndex < _unitButtonList.Count; buttonIndex++)
            {
                var button = _unitButtonList[buttonIndex];
                button.Rect = new Rectangle(contentRect.Left + 200, contentRect.Top + 200 + buttonIndex * 21, 100, 20);
                button.Draw(spriteBatch);
            }

            if (_selectedCharacter is not null)
            {
                var unitName = _selectedCharacter.UnitScheme.Name;
                var name = GameObjectHelper.GetLocalized(unitName);

                var sb = new List<string>
                {
                    name,
                    string.Format(UiResource.HitPointsLabelTemplate, _selectedCharacter.MaxHitPoints),
                    string.Format(UiResource.ManaLabelTemplate, _selectedCharacter.ManaPool,
                        _selectedCharacter.ManaPoolSize),
                    string.Format(UiResource.CombatLevelLavelTemplate, _selectedCharacter.Level, 0,
                        _selectedCharacter.LevelUpXp),
                    string.Format(UiResource.EquipmentLevelLavelTemplate, _selectedCharacter.EquipmentLevel,
                        0,
                        _selectedCharacter.EquipmentLevelup)
                };

                foreach (var skill in _selectedCharacter.Skills)
                {
                    var skillNameText = GameObjectResources.ResourceManager.GetString(skill.Sid.ToString()) ??
                                        $"#Resource-{skill.Sid}";

                    sb.Add(skillNameText);
                    if (skill.ManaCost is not null)
                    {
                        sb.Add(string.Format(UiResource.ManaCostLabelTemplate, skill.ManaCost));
                    }

                    // TODO Display skill efficient - damages, durations, etc.
                }

                for (var statIndex = 0; statIndex < sb.Count; statIndex++)
                {
                    var line = sb[statIndex];
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), line,
                        new Vector2(contentRect.Center.X, contentRect.Top + statIndex * 22), Color.White);
                }

                for (var buttonIndex = 0; buttonIndex < _slotButtonList.Count; buttonIndex++)
                {
                    var button = _slotButtonList[buttonIndex];
                    button.Rect = new Rectangle(contentRect.Center.X,
                        contentRect.Top + sb.Count * 22 + buttonIndex * 21, 100, 20);
                    button.Draw(spriteBatch);
                }
            }

            var array = _globeProvider.Globe.Player.Inventory.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                var resourceItem = array[i];
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{resourceItem.Type} x {resourceItem.Amount}", new Vector2(100, i * 20 + 100), Color.Wheat);
            }

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            if (!_isInitialized)
            {
                var globe = _globeProvider.Globe;
                var playerCharacters = globe.Player.Party.GetUnits().Concat(globe.Player.Pool.Units).ToArray();

                var biomeButton = new ResourceTextButton(nameof(UiResource.BackToMapMenuButtonTitle),
                    _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(),
                    Rectangle.Empty);
                biomeButton.OnClick += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
                };
                _navigationButtonList.Add(biomeButton);

                foreach (var character in playerCharacters)
                {
                    var unitName = character.UnitScheme.Name;
                    var name = GameObjectHelper.GetLocalized(unitName);

                    var button = new TextButton(name, _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(), new Rectangle());
                    button.OnClick += (_, _) =>
                    {
                        _selectedCharacter = character;

                        InitSlotAssignmentButtons(character, _globeProvider.Globe.Player);

                        InitUpgrageButtons(character, _globeProvider.Globe.Player);
                    };
                    _unitButtonList.Add(button);
                }

                _isInitialized = true;
            }
            else
            {
                foreach (var button in _navigationButtonList)
                {
                    button.Update(_resolutionIndependentRenderer);
                }

                foreach (var button in _unitButtonList)
                {
                    button.Update(_resolutionIndependentRenderer);
                }

                foreach (var button in _slotButtonList.ToArray())
                {
                    button.Update(_resolutionIndependentRenderer);
                }
            }
        }

        private void InitUpgrageButtons(Unit character, Player player)
        {

            var xpAmount = player.Inventory.Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount;
            if (xpAmount >= character.LevelUpXp)
            {
                var levelUpButton = new TextButton("Level Up", _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), Rectangle.Empty);

                levelUpButton.OnClick += (_, _) =>
                {
                    player.Inventory.Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount -= character.LevelUpXp;
                    character.Level++;
                    InitUpgrageButtons(character, player);
                };

                _slotButtonList.Add(levelUpButton);
            }

            var equipmentType = UnsortedHelpers.GetEquipmentItemTypeByUnitScheme(character.UnitScheme);
            if (equipmentType is not null)
            {
                var equipmentResource = player.Inventory.Single(x => x.Type == equipmentType.Value).Amount;
                if (equipmentResource >= character.EquipmentLevelup)
                {
                    var levelUpButton = new TextButton("Equipment Upgrade", _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), Rectangle.Empty);
                    levelUpButton.OnClick += (_, _) =>
                    {
                        player.Inventory.Single(x => x.Type == equipmentType.Value).Amount -= character.EquipmentLevelup;
                        character.EquipmentLevel++;
                        InitUpgrageButtons(character, player);
                    };
                    _slotButtonList.Add(levelUpButton);
                }
            }
        }

        private bool GetIsCharacterInGroup()
        {
            return _globeProvider.Globe.Player.Party.GetUnits().Contains(_selectedCharacter);
        }

        private void InitSlotAssignmentButtons(Unit character, Player player)
        {
            _slotButtonList.Clear();

            var isCharacterInGroup = GetIsCharacterInGroup();
            if (isCharacterInGroup)
            {
                var reserveButton = new ResourceTextButton(
                    nameof(UiResource.MoveToThePoolButtonTitle),
                    _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(),
                    Rectangle.Empty);
                _slotButtonList.Add(reserveButton);

                reserveButton.OnClick += (_, _) =>
                {
                    player.MoveToPool(character);

                    InitSlotAssignmentButtons(character, _globeProvider.Globe.Player);
                };
            }
            else
            {
                var freeSlots = player.Party.GetFreeSlots();
                foreach (var slot in freeSlots)
                {
                    var slotButton = new TextButton(slot.Index.ToString(),
                        _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(),
                        Rectangle.Empty);

                    _slotButtonList.Add(slotButton);

                    slotButton.OnClick += (_, _) =>
                    {
                        player.MoveToParty(character, slot.Index);

                        InitSlotAssignmentButtons(character, _globeProvider.Globe.Player);
                    };
                }
            }
        }
    }
}