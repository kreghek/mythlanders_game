using CombatDicesTeam.GenericRanges;

namespace Core.PropDrop;

/// <summary>
/// Запись в таблице дропа.
/// </summary>
public interface IDropTableRecordSubScheme
{
    /// <summary>
    /// Possible resource count.
    /// </summary>
    /// <remarks>
    /// Используется только ресурсами.
    /// </remarks>
    GenericRange<int> Count { get; }

    /// <summary>
    /// Дополнительный дроп к текущему.
    /// </summary>
    IDropTableScheme[]? Extra { get; }

    /// <summary>
    /// Идентификатор схемы предмета.
    /// </summary>
    /// <remarks>
    /// Если указано null, то никакой предмет не дропается. null - это ничего не дропнулось.
    /// </remarks>
    string? SchemeSid { get; }

    /// <summary>
    /// Вес записи в таблице дропа.
    /// </summary>
    /// <remarks>
    /// Чем выше, тем веротянее будет выбрана данная запись при разрешении дропа.
    /// </remarks>
    int Weight { get; }
}

public sealed record DropTableRecordSubScheme(IDropTableScheme[]? Extra, GenericRange<int> Count,
    string? SchemeSid, int Weight) : IDropTableRecordSubScheme;