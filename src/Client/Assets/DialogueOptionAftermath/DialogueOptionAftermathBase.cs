using System.Collections.Generic;

using Client.GameScreens;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal abstract class DialogueOptionAftermathBase<TAftermathContext> : IDialogueOptionAftermath<TAftermathContext>
{
    protected abstract IReadOnlyList<object> GetDescriptionValues(TAftermathContext aftermathContext);

    protected virtual string GetSid()
    {
        return GetType().Name[..^"OptionAftermath".Length];
    }

    /// <inheritdoc />
    public string GetDescription(TAftermathContext aftermathContext)
    {
        if (IsHidden)
        {
            return string.Empty;
        }

        var descriptionTemplate = GameObjectHelper.GetLocalized(GetSid() + "_Description");

        var values = GetDescriptionValues(aftermathContext);

        return string.Format(descriptionTemplate, values);
    }

    /// <inheritdoc />
    public bool IsHidden { get; set; }

    /// <inheritdoc />
    public abstract void Apply(TAftermathContext aftermathContext);
}