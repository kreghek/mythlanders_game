using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class ChangeCharacterRelationsOptionAftermath : DialogueOptionAftermathBase
{
    private readonly CharacterKnowledgeLevel _knowledgeLevel;
    private readonly UnitName _targetCharacter;

    public ChangeCharacterRelationsOptionAftermath(UnitName targetCharacter, CharacterKnowledgeLevel knowledgeLevel)
    {
        _targetCharacter = targetCharacter;
        _knowledgeLevel = knowledgeLevel;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.ChangeCharacterRelations(_targetCharacter, _knowledgeLevel);
    }

    protected override IReadOnlyList<string> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return new[]
        {
            _targetCharacter.ToString(),
            _knowledgeLevel.ToString()
        };
    }
}