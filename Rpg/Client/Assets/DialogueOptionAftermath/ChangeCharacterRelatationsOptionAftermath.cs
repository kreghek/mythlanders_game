using Client.Core;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class ChangeCharacterRelatationsOptionAftermath : IDialogueOptionAftermath
{
    private readonly CharacterKnowledgeLevel _knowledgeLevel;
    private readonly UnitName _targetCharacter;

    public ChangeCharacterRelatationsOptionAftermath(UnitName targetCharacter, CharacterKnowledgeLevel knowledgeLevel)
    {
        _targetCharacter = targetCharacter;
        _knowledgeLevel = knowledgeLevel;
    }

    public void Apply(IEventContext dialogContext)
    {
        // TODO Not implemented
    }
}