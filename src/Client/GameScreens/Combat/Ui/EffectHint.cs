using System.Linq;

using Client.Engine;
using Client.GameScreens.Common.SkillEffectDrawers;

using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal class EffectHint : HintBase
{
    private readonly ICombatantStatus _effect;
    private readonly ISkillEffectDrawer[] _effectDrawers;

    public EffectHint(ICombatantStatus effect)
    {
        var font = UiThemeManager.UiContentStorage.GetMainFont();
        _effect = effect;
        _effectDrawers = EffectDrawersCollector.GetDrawersInAssembly(font).ToArray();
    }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
    {
        foreach (var effectDrawer in _effectDrawers)
        {
            //if (effectDrawer.Draw(spriteBatch, _effect, clientRect.Location.ToVector2()))
            //{
            //    break;
            //}
        }
    }
}