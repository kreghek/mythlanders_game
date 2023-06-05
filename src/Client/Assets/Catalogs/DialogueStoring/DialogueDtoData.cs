using JetBrains.Annotations;

namespace Client.Assets.Catalogs.DialogueStoring;

/// <summary>
/// Structure to deserialize common dialogue data.
/// </summary>
[UsedImplicitly]
internal sealed class DialogueDtoData
{
    public string Data { get; [UsedImplicitly] init; } = null!;
    public string Type { get; [UsedImplicitly] init; } = null!;
}