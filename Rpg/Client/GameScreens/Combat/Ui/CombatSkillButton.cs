﻿using Client.Engine;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatSkillButton : EntityButtonBase<CombatMovementInstance>
{
    private readonly Texture2D _icon;
    private readonly Rectangle? _iconRect;
    private readonly ISkillPanelState _skillPanelState;

    private float _counter;

    public CombatSkillButton(IconData iconData, CombatMovementInstance combatMovement,
        ISkillPanelState skillPanelState) : base(combatMovement)
    {
        _icon = iconData.Spritesheet;
        _iconRect = iconData.SourceRect;
        _skillPanelState = skillPanelState;
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.Skill;
    }

    protected override Color CalculateColor()
    {
        if (Entity != _skillPanelState.SelectedCombatMovement)
        {
            return base.CalculateColor();
        }

        return Color.Lerp(Color.White, Color.Cyan, _counter);
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        if (IsEnabled)
        {
            spriteBatch.Draw(_icon, contentRect, _iconRect, color);
            DrawBackground(spriteBatch, color);
        }
        else
        {
            var disabledColor = Color.Lerp(color, Color.Red, 0.5f + _counter * 0.5f);
            spriteBatch.Draw(_icon, contentRect, _iconRect, disabledColor);
            DrawBackground(spriteBatch, disabledColor);
        }
    }

    protected override void UpdateContent()
    {
        base.UpdateContent();

        _counter += 0.005f;
        if (_counter > 0.5f)
        {
            _counter = 0.0f;
        }
    }
}