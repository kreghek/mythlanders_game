using System.Collections.Generic;

using Client.GameScreens.PreHistory;

namespace Client.Assets.DialogueOptionAftermath.PreHistory;

internal sealed class AddHeroOptionAftermath : PreHistoryDialogueOptionAftermathBase
{
    private readonly string _heroSid;

    public AddHeroOptionAftermath(string heroSid)
    {
        _heroSid = heroSid;
    }

    public override void Apply(PreHistoryAftermathContext aftermathContext)
    {
        aftermathContext.AddNewHero(_heroSid);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(PreHistoryAftermathContext aftermathContext)
    {
        return new object[]
        {
            _heroSid
        };
    }
}