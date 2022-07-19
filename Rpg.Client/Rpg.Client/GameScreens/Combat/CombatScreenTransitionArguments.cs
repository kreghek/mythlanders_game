using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Combat
{
    internal sealed class CombatScreenTransitionArguments : IScreenTransitionArguments
    {
        public CombatSequence CombatSequence { get; init; }
        public int CurrentCombatIndex { get; init; }
        public bool IsAutoplay { get; init; }
        public GlobeNode Location { get; init; }
        public Dialogue? VictoryDialogue { get; init; }
        public bool VictoryDialogueIsStartEvent { get; init; }
    }
}