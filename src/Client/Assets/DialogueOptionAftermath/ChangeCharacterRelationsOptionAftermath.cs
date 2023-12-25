using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

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

    protected override IReadOnlyList<object> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return new object[]
        {
            _targetCharacter,
            _knowledgeLevel
        };
    }
}