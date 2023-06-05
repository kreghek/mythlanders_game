using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.GameScreens.Map.GameObjects;

namespace Rpg.Client.GameScreens.Map.Ui
{
    internal sealed class LocationInfoModalContext
    {
        public CombatDelegate AutoCombatDelegate { get; set; }
        public Event? AvailableEvent { get; internal set; }
        public CombatDelegate CombatDelegate { get; set; }
        public Globe Globe { get; set; }
        public GlobeNodeMarkerGameObject SelectedNodeGameObject { get; set; }
    }

    internal delegate void CombatDelegate(GlobeNode selectedLocation, Event? assignedDialgueEvent);
}