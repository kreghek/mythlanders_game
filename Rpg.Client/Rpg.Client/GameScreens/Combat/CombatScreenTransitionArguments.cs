using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.ScreenManagement;

using static Rpg.Client.Core.Combat;

namespace Rpg.Client.GameScreens.Combat
{
    internal sealed class CombatScreenTransitionArguments : IScreenTransitionArguments
    {
        public HeroCampaign CurrentCampaign { get; set; }
        public CombatSequence CombatSequence { get; init; }
        public int CurrentCombatIndex { get; init; }
        public bool IsAutoplay { get; init; }
        public GlobeNode Location { get; init; }

        public IReadOnlyCollection<HeroHp> StartHpItems { get; init; }
        public Dialogue? VictoryDialogue { get; init; }
        public bool VictoryDialogueIsStartEvent { get; init; }
    }
}