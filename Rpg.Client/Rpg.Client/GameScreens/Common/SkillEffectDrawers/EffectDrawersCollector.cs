using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal static class EffectDrawersCollector
    {
        public static IReadOnlyCollection<ISkillEffectDrawer> GetDrawersInAssembly(SpriteFont font)
        {
            var assembly = typeof(ISkillEffectDrawer).Assembly;
            var drawerTypes = assembly.GetTypes().Where(t =>
                typeof(ISkillEffectDrawer).IsAssignableFrom(t) && t != typeof(ISkillEffectDrawer));
            var drawers = drawerTypes.Select(t => Activator.CreateInstance(t, new object[] { font }))
                .OfType<ISkillEffectDrawer>();
            return drawers.ToArray();
        }
    }
}