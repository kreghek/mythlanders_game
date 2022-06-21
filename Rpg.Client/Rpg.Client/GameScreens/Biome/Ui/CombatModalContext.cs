using System;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.GameScreens.Biome.GameObjects;

namespace Rpg.Client.GameScreens.Biome.Ui
{
    internal sealed class CombatModalContext
    {
        public Action<GlobeNode, Event> AutoCombatDelegate { get; set; }
        public Event? AvailableEvent { get; internal set; }
        public Action<GlobeNode, Event> CombatDelegate { get; set; }
        public Globe Globe { get; set; }
        public GlobeNodeMarkerGameObject SelectedNodeGameObject { get; set; }
    }
}