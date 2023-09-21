using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.GameScreens;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal abstract class DialogueOptionAftermathBase: IDialogueOptionAftermath<AftermathContext>
{
    protected virtual string GetSid() => this.GetType().Name[..^"OptionAftermath".Length];

    /// <inheritdoc />
    public string GetDescription(AftermathContext aftermathContext)
    {
        if (IsHidden)
        {
            return String.Empty;
        }

        var descriptionTemplate = GameObjectHelper.GetLocalized(GetSid() + "_Description");

        var values = GetDescriptionValues(aftermathContext);

        return string.Format(descriptionTemplate, values);
    }

    /// <inheritdoc />
    public bool IsHidden { get; set; }

    /// <inheritdoc />
    public abstract void Apply(AftermathContext aftermathContext);

    protected abstract IReadOnlyList<string> GetDescriptionValues(AftermathContext aftermathContext);
}