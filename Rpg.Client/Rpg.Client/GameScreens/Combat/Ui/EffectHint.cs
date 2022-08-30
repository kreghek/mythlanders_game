using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common.SkillEffectDrawers;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class EffectHint : HintBase
    {
        private readonly EffectBase _effect;
        private readonly ISkillEffectDrawer[] _effectDrawers;

        public EffectHint(EffectBase effect)
        {
            var font = UiThemeManager.UiContentStorage.GetMainFont();
            _effect = effect;
            _effectDrawers = EffectDrawersCollector.GetDrawersInAssembly(font).ToArray();
        }

        protected override Point CalcTextureOffset() => Point.Zero;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            foreach (var effectDrawer in _effectDrawers)
            {
                if (effectDrawer.Draw(spriteBatch, _effect, SkillDirection.Self, clientRect.Location.ToVector2()))
                {
                    break;
                }
            }
        }
    }
}