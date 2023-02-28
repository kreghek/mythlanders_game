using Rpg.Client.Core;

namespace Client.Assets.StoryPointJobs;

/// <summary>
/// Область действия работы.
/// </summary>
internal static class JobScopeCatalog
{
    /// <summary>
    /// Общая область действия.
    /// Прогресс не будет сбрасываться после окончания боя.
    /// Не влияет на одноходовые задачи (одним ударом убить 2 противника)
    /// </summary>
    public static IJobScope Global { get; } = new JobScope();

    /// <summary>
    /// Область действия на высадку.
    /// Если работы в рамках одной высадки не выполнены, то прогресс будет сброшен.
    /// </summary>
    public static IJobScope Combat { get; } = new JobScope();
    
    /// <summary>
    /// Complete sets of jobs during current campaign. 
    /// </summary>
    public static IJobScope Campaign { get; } = new JobScope();
}