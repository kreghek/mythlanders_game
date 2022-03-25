using System;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Locations.Ui
{
    internal sealed class CombatModalContext
    {
        public Action<GlobeNode> AutoCombatDelegate { get; set; }
        public Action<GlobeNode> CombatDelegate { get; set; }
        public Globe Globe { get; set; }

        public GlobeNode GlobeNode { get; set; }
    }
}