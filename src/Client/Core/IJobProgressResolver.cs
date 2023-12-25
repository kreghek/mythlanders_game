namespace Client.Core;

public interface IJobProgressResolver
{
    /// <summary>
    /// Применяет прогресс к текущим работам.
    /// </summary>
    /// <param name="progress"> Объект прогресса. </param>
    /// <param name="evolutionData"> Данные о развитии персонажа. </param>
    /// <returns> Возвращает true, если все работы выполнены. </returns>
    void ApplyProgress(IJobProgress progress, IJobExecutable target);
}