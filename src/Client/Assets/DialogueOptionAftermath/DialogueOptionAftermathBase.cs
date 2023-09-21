using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.GameScreens;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

public abstract class DialogueOptionAftermathBase: IDialogueOptionAftermath<AftermathContext>
{
    protected virtual string GetSid() => this.GetType().Name[..^"OptionAftermath".Length]; 

    public string GetDescription(AftermathContext aftermathContext)
    {
        var descriptionTemplate = GameObjectHelper.GetLocalized(GetSid() + "_Description");
        
        var values = GetDescriptionValues(aftermathContext);

        return string.Format(descriptionTemplate, values);
    }

    public abstract void Apply(AftermathContext aftermathContext);

    protected abstract IReadOnlyList<string> GetDescriptionValues(AftermathContext aftermathContext);
}