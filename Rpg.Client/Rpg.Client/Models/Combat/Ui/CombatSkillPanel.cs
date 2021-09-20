using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Core.Effects;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal class CombatSkillPanel
    {
        private readonly IDictionary<ButtonBase, CombatSkillCard> _buttonCombatPowerDict;
        private readonly IList<IconButton> _buttons;
        private readonly IUiContentStorage _uiContentStorage;

        private ButtonBase? _hoverButton;
        private CombatSkillCard _selectedCard;
        private CombatUnit? _unit;

        public CombatSkillPanel(IUiContentStorage uiContentStorage)
        {
            _buttons = new List<IconButton>();
            _buttonCombatPowerDict = new Dictionary<ButtonBase, CombatSkillCard>();

            _uiContentStorage = uiContentStorage;

            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }

        public CombatSkillCard? SelectedCard
        {
            get => _selectedCard;
            set
            {
                if (_selectedCard == value)
                {
                    return;
                }

                _selectedCard = value;

                CardSelected?.Invoke(this, _selectedCard);
            }
        }

        public CombatUnit? Unit
        {
            get => _unit;
            set
            {
                if (_unit == value)
                {
                    return;
                }

                _unit = value;

                RecreateButtons();
            }
        }

        internal void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (!IsEnabled)
            {
                return;
            }

            var buttonWidth = _buttons.Count * 32;
            for (var buttonIndex = 0; buttonIndex < _buttons.Count; buttonIndex++)
            {
                var button = _buttons[buttonIndex];
                button.Rect = GetButtonRectangle(graphicsDevice, buttonWidth, buttonIndex);
                button.Draw(spriteBatch);
            }

            if (_hoverButton is not null)
            {
                var combatPower = _buttonCombatPowerDict[_hoverButton];

                var hintPosition = _hoverButton.Rect.Location.ToVector2() - new Vector2(100, 105);
                var hintRectangle = new Rectangle(hintPosition.ToPoint(), new Point(100, 100));
                spriteBatch.Draw(_uiContentStorage.GetButtonTexture(), hintRectangle, Color.White);
                var skillTitlePosition = hintRectangle.Location.ToVector2() + new Vector2(0, 5);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), combatPower.Skill.Sid, skillTitlePosition,
                    Color.Black);

                var ruleBlockPosition = skillTitlePosition + new Vector2(0, 10);
                var skillRules = combatPower.Skill.Rules.ToArray();
                for (var ruleIndex = 0; ruleIndex < skillRules.Length; ruleIndex++)
                {
                    var rule = skillRules[ruleIndex];
                    var effectCreator = rule.EffectCreator;
                    var effectToDisplay = effectCreator.CreateToDisplay(Unit);

                    var rulePosition = ruleBlockPosition + new Vector2(0, 10) * ruleIndex;

                    if (effectToDisplay is AttackEffect attackEffect)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                            $"Damage: {attackEffect.MinDamage} - {attackEffect.MaxDamage} to {rule.Direction}",
                            rulePosition, Color.Black);
                    }
                    else if (effectToDisplay is HealEffect healEffect)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                            $"Heal: {healEffect.MinHeal} - {healEffect.MaxHeal}", rulePosition, Color.Black);
                    }
                    else if (effectToDisplay is PeriodicHealEffect)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Heal over time", rulePosition,
                            Color.Black);
                    }
                    else if (effectToDisplay is DopeHerbEffect)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Stun", rulePosition, Color.Black);
                    }
                    else if (effectToDisplay is PowerUpEffect)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Power up", rulePosition, Color.Black);
                    }
                }
            }
        }

        internal void Update()
        {
            if (!IsEnabled)
            {
                return;
            }

            var mouse = Mouse.GetState();
            var mouseRect = new Rectangle(mouse.Position, new Point(1, 1));

            _hoverButton = null;
            foreach (var button in _buttons)
            {
                button.Update();

                if (mouseRect.Intersects(button.Rect))
                {
                    _hoverButton = button;
                }
            }
        }

        private void CombatPowerButton_OnClick(object? sender, EventArgs e)
        {
            if (sender is null)
            {
                Debug.Fail("Sender mustn't be null.");
            }

            var combatPowerCard = _buttonCombatPowerDict[(ButtonBase)sender];
            SelectedCard = combatPowerCard;
        }

        private static Rectangle GetButtonRectangle(GraphicsDevice graphicsDevice, int buttonWidth, int i)
        {
            return new Rectangle(graphicsDevice.Viewport.Bounds.Center.X - buttonWidth / 2 + 32 * i,
                graphicsDevice.Viewport.Bounds.Bottom - 32, 32, 32);
        }

        private static int? GetIconIndex(string sid)
        {
            return sid switch
            {
                "Slash" => 0,
                "Defence Stance" => 1,
                "Wide Slash" => 2,
                "Strike" => 3,
                "Arrow Rain" => 4,
                "Zduhach Might" => 5,
                "Heal" => 6,
                "Periodic Heal" => 6,
                "Dope Herb" => 7,
                "Mass Stun" => 7,
                "Mass Heal" => 8,
                "Power Up" => 1,
                _ => null
            };
        }

        private static Rectangle GetIconRect(string sid)
        {
            const int SPRITESHEET_COLUMN_COUNT = 3;
            const int ICON_SIZE = 32;

            var iconIndexNullable = GetIconIndex(sid);

            Debug.Assert(iconIndexNullable is not null, $"Don't forget add combat power in {nameof(GetIconIndex)}");

            var iconIndex = iconIndexNullable is not null ? iconIndexNullable.Value : 0;

            var x = iconIndex % SPRITESHEET_COLUMN_COUNT;
            var y = iconIndex / SPRITESHEET_COLUMN_COUNT;
            var rect = new Rectangle(x * ICON_SIZE, y * ICON_SIZE, ICON_SIZE, ICON_SIZE);

            return rect;
        }

        private void RecreateButtons()
        {
            _buttons.Clear();
            _buttonCombatPowerDict.Clear();
            SelectedCard = null;

            if (_unit is null)
            {
                SelectedCard = null;
                return;
                //throw new InvalidOperationException("Unit required to be initialized before.");
            }

            if (_unit.CombatCards is null)
            {
                throw new InvalidOperationException($"The unit {_unit} required to have got combat powers.");
            }

            foreach (var card in _unit.CombatCards)
            {
                var iconRect = GetIconRect(card.Skill.Sid);
                var iconData = new IconData(_uiContentStorage.GetCombatPowerIconsTexture(), iconRect);
                var button = new IconButton(_uiContentStorage.GetButtonTexture(), iconData, Rectangle.Empty);
                _buttons.Add(button);
                _buttonCombatPowerDict[button] = card;
                button.OnClick += CombatPowerButton_OnClick;
            }
        }

        public event EventHandler<CombatSkillCard?>? CardSelected;
    }
}