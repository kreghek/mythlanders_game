using System;

using Rpg.Client.Core;
using Rpg.Client.GameScreens.Biome.GameObjects;

namespace Rpg.Client.GameScreens.Biome.Ui
{
    internal sealed class CombatModalContext
    {
        public Action<GlobeNode, Core.Event> AutoCombatDelegate { get; set; }
        public Action<GlobeNode, Core.Event> CombatDelegate { get; set; }
        public Globe Globe { get; set; }
        public GlobeNodeMarkerGameObject SelectedNodeGameObject { get; set; }
        public Core.Event? AvailableEvent { get; internal set; }
    }
}