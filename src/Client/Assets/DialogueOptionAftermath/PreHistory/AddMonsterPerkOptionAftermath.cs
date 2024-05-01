using System.Collections.Generic;

using Client.GameScreens.PreHistory;

namespace Client.Assets.DialogueOptionAftermath.PreHistory;

internal sealed class AddMonsterPerkOptionAftermath : PreHistoryDialogueOptionAftermathBase
{
    private readonly string _perkSid;

    public AddMonsterPerkOptionAftermath(string perkSid)
    {
        _perkSid = perkSid;
    }

    public override void Apply(PreHistoryAftermathContext aftermathContext)
    {
        aftermathContext.AddMonsterPerk(_perkSid);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(PreHistoryAftermathContext aftermathContext)
    {
        return new object[]
        {
            _perkSid
        };
    }
}