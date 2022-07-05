using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Speech
{
    internal sealed class SpeechScreenTransitionArgs : IScreenTransitionArguments
    {
        public Dialogue CurrentDialogue { get; init; }
        public Dialogue? CombatVictoryDialogue { get; init; }
        public GlobeNode Location { get; init; }
        public bool IsCombatPreparingDialogue => NextCombats is not null;
        public bool IsStartDialogueEvent { get; init; }
        public CombatSequence? NextCombats { get; init; }
    }
}