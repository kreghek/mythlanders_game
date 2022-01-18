using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.CharacterDetails
{
    internal sealed class CharacterDetailsScreen: GameScreenWithMenuBase
    {
        private readonly IUiContentStorage _uiContentStorage;
        private readonly GameObjectContentStorage _gameObjectsContentStorage;
        private readonly ScreenService _screenService;
        private readonly IList<ButtonBase> _slotButtonList;
        private readonly GlobeProvider _globeProvider;

        public CharacterDetailsScreen(EwarGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _gameObjectsContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _screenService = game.Services.GetService<ScreenService>();
            
            _slotButtonList = new List<ButtonBase>();
            
            _globeProvider = game.Services.GetService<GlobeProvider>();
            
            InitSlotAssignmentButtons(_screenService.Selected, _globeProvider.Globe.Player);
        }

        private bool GetIsCharacterInGroup(Unit selectedCharacter)
        {
            return _globeProvider.Globe.Player.Party.GetUnits().Contains(selectedCharacter);
        }
        
        
        private IEnumerable<GroupSlot> GetAvailableSlots(IEnumerable<GroupSlot> freeSlots)
        {
            if (_globeProvider.Globe.Player.Abilities.Contains(PlayerAbility.AvailableTanks))
            {
                return freeSlots;
            }

            // In the first biome the player can use only first 3 slots.
            // There is no ability to split characters on tank line and dd+support.
            return freeSlots.Where(x => !x.IsTankLine);
        }
        
        private void InitSlotAssignmentButtons(Unit character, Player player)
        {
            _slotButtonList.Clear();

            var isCharacterInGroup = GetIsCharacterInGroup(character);
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

                    InitSlotAssignmentButtons(character, player);
                };
            }
            else
            {
                var freeSlots = player.Party.GetFreeSlots();
                var availableSlots = GetAvailableSlots(freeSlots);
                foreach (var slot in availableSlots)
                {
                    var slotButton = new TextButton(slot.Index.ToString(),
                        _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(),
                        Rectangle.Empty);

                    _slotButtonList.Add(slotButton);

                    slotButton.OnClick += (_, _) =>
                    {
                        player.MoveToParty(character, slot.Index);

                        InitSlotAssignmentButtons(character, player);
                    };
                }
            }

            InitUpgradeButtons(character, player);
        }
        
        private void InitUpgradeButtons(Unit character, Player player)
        {
            var xpAmount = player.Inventory.Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount;
            if (xpAmount >= character.LevelUpXpAmount)
            {
                var levelUpButton = new TextButton("Level Up", _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), Rectangle.Empty);

                levelUpButton.OnClick += (_, _) =>
                {
                    player.Inventory.Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount -=
                        character.LevelUpXpAmount;
                    character.LevelUp();
                    InitSlotAssignmentButtons(character, player);
                };

                _slotButtonList.Add(levelUpButton);
            }

            foreach (var equipment in character.Equipments)
            {
                var resourceItem = player.Inventory.Single(x => x.Type == equipment.Scheme.RequiredResourceToLevelUp);
                var equipmentResourceAmount = resourceItem.Amount;
                if (equipmentResourceAmount >= equipment.RequiredResourceAmountToLevelUp)
                {
                    var levelUpButton = new TextButton($"Upgrade {equipment.Scheme.Sid} to level {equipment.Level + 1}",
                        _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(), Rectangle.Empty);
                    levelUpButton.OnClick += (_, _) =>
                    {
                        resourceItem.Amount -= equipment.RequiredResourceAmountToLevelUp;
                        equipment.LevelUp();
                        InitSlotAssignmentButtons(character, player);
                    };
                    _slotButtonList.Add(levelUpButton);
                }
            }
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle),
                _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont());
            backButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Party);
            };

            return new ButtonBase[] { backButton };
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

            var _selectedCharacter = _screenService.Selected;

            var unitName = _selectedCharacter.UnitScheme.Name;
            var name = GameObjectHelper.GetLocalized(unitName);

            var sb = new List<string>
            {
                name,
                string.Format(UiResource.HitPointsLabelTemplate, _selectedCharacter.MaxHitPoints),
                string.Format(UiResource.ManaLabelTemplate, _selectedCharacter.ManaPool,
                    _selectedCharacter.ManaPoolSize),
                string.Format(UiResource.CombatLevelTemplate, _selectedCharacter.Level),
                string.Format(UiResource.CombatLevelUpTemplate, _selectedCharacter.LevelUpXpAmount)
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

            foreach (var perk in _selectedCharacter.Perks)
            {
                var localizedName = GameObjectResources.ResourceManager.GetString(perk.GetType().Name);
                sb.Add(localizedName ?? $"[{perk.GetType().Name}]");

                var localizedDescription =
                    GameObjectResources.ResourceManager.GetString($"{perk.GetType().Name}Description");
                if (localizedDescription is not null)
                {
                    sb.Add(localizedDescription);
                }
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

            var array = _globeProvider.Globe.Player.Inventory.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                var resourceItem = array[i];
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{resourceItem.Type} x {resourceItem.Amount}",
                    new Vector2(100, i * 20 + 100), Color.Wheat);
            }

            spriteBatch.End();
        }
    }
}