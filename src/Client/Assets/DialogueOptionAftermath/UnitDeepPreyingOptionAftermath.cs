using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.GlobalEffects;
using Client.Core;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class UnitDeepPreyingOptionAftermath : DialogueOptionAftermathBase
{
    private readonly UnitName _name;

    public UnitDeepPreyingOptionAftermath(UnitName name)
    {
        _name = name;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        var globalEvent = new CharacterDeepPreyingGlobeEvent(_name);

        aftermathContext.AddNewGlobalEvent(globalEvent);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return new object[]
        {
            _name
        };
    }
}