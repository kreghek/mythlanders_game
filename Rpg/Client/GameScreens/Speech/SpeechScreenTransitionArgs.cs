using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Speech
{
    internal sealed class SpeechScreenTransitionArgs : IScreenTransitionArguments
    {
        public Dialogue? CombatVictoryDialogue { get; init; }
        public Dialogue CurrentDialogue { get; init; }
        public bool IsCombatPreparingDialogue => NextCombats is not null;
        public bool IsStartDialogueEvent { get; init; }
        public GlobeNodeSid Location { get; init; }
        public CombatSequence? NextCombats { get; init; }
    }
}