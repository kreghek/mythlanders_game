using Client.Core;
using Client.Core.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class ChangeCharacterRelatationsOptionAftermath : IDialogueOptionAftermath<AftermathContext>
{
    private readonly CharacterKnowledgeLevel _knowledgeLevel;
    private readonly UnitName _targetCharacter;

    public ChangeCharacterRelatationsOptionAftermath(UnitName targetCharacter, CharacterKnowledgeLevel knowledgeLevel)
    {
        _targetCharacter = targetCharacter;
        _knowledgeLevel = knowledgeLevel;
    }

    public void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.ChangeCharacterRelations(_targetCharacter, _knowledgeLevel);
    }
}