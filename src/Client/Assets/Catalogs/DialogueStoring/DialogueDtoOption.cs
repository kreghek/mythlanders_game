using JetBrains.Annotations;

namespace Client.Assets.Catalogs.DialogueStoring;

/// <summary>
/// Used to deserialize dialogue from file.
/// </summary>
[UsedImplicitly]
internal class DialogueDtoOption
{
    public DialogueDtoData[]? Aftermaths { get; [UsedImplicitly] init; }

    // ReSharper disable once UnusedMember.Global
    // This member is not used but required to deserialization.
    public string Description { get; [UsedImplicitly] init; } = null!;
    public DialogueDtoData[]? HideConditions { get; [UsedImplicitly] init; }
    public string? Next { get; [UsedImplicitly] init; }
    public DialogueDtoData[]? SelectConditions { get; [UsedImplicitly] init; }

    // ReSharper disable once UnusedMember.Global
    // This member is not used but required to deserialization.
    public string Text { get; [UsedImplicitly] init; } = null!;
}