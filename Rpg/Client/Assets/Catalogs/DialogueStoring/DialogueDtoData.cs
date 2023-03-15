using JetBrains.Annotations;

namespace Client.Assets.Catalogs.DialogueStoring;

/// <summary>
/// Structure to deserialize common dialogue data.
/// </summary>
[UsedImplicitly]
internal sealed record DialogueDtoData(string Type, string Data);