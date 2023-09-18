using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Heroes;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class AddHeroOptionAftermath : IDialogueOptionAftermath<AftermathContext>
{
    private readonly UnitScheme _scheme;

    public AddHeroOptionAftermath(UnitScheme scheme)
    {
        _scheme = scheme;
    }

    public void Apply(AftermathContext aftermathContext)
    {
        const int DEFAULT_LEVEL = 1;
        var unit = new Hero(_scheme, DEFAULT_LEVEL)
        {
            IsPlayerControlled = true
        };
        aftermathContext.AddNewCharacter(unit);
    }
}