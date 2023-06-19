using System.Diagnostics.CodeAnalysis;

namespace Core.Dices;

/// <summary>
/// The random number generator - dice.
/// </summary>
public interface IDice
{
    /// <summary>
    /// Get roll of the dice. Min is 1.
    /// </summary>
    /// <param name="n"> Edges of the dice. </param>
    /// <returns> The roll result. </returns>
    int Roll(int n);
}

/// <summary>
/// The implementation of the dice working by linear law.
/// </summary>
public class LinearDice : IDice
{
    private readonly Random _random;

    /// <summary>
    /// Конструктор кости.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public LinearDice()
    {
        _random = new Random();
    }

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="seed"> The randomization seed. </param>
    /// <remarks>
    /// Same seeds make same random number sequence.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public LinearDice(int seed)
    {
        _random = new Random(seed);
    }

    /// <inheritdoc />
    public int Roll(int n)
    {
        var rollResult = _random.Next(1, n + 1);
        return rollResult;
    }
}

/// <summary>
/// Вспомогательные расширения сервиса для работы с игральной костью.
/// </summary>
public static class DiceExtensions
{
    /// <summary>
    /// Получение случайного числа в указаном диапазоне [min, max].
    /// </summary>
    /// <param name="dice"> Используемая для броска кость. </param>
    /// <param name="min"> Минимальное значение. </param>
    /// <param name="max"> Максимальное значение. </param>
    /// <returns> Возвращает случайное число в указанном диапазоне [min, max]. </returns>
    public static int Roll(this IDice dice, int min, int max)
    {
        if (min > max)
        // ReSharper disable once LocalizableElement
        // Exception's messages shouldn't be localized.
        {
            throw new ArgumentException($"Max value {max} can't be least min {min}.",
                nameof(max));
        }

        if (dice == null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        if (min == max)
        {
            return min;
        }

        var range = max - min;
        var roll = dice.Roll(range + 1);

        return (roll - 1) + min;
    }

    public static int Roll2D6(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return dice.Roll(6) + dice.Roll(6);
    }

    /// <summary>
    /// Выбирает случайный индекс из набора.
    /// </summary>
    /// <typeparam name="T"> Тип элементов списка. </typeparam>
    /// <param name="dice"> Кость, на основе которой делать случайный выбор. </param>
    /// <param name="list"> Список элементов, из которого выбирать элемент. </param>
    /// <returns> Случайный элемент из списка. </returns>
    public static int RollArrayIndex<T>(this IDice dice, IList<T> list)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var rollIndex = dice.Roll(0, list.Count - 1);
        return rollIndex;
    }

    public static int RollD100(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return dice.Roll(100);
    }

    public static int RollD3(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return dice.Roll(3);
    }

    public static int RollD6(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return dice.Roll(6);
    }

    /// <summary>
    /// Выбирает случайное значение из списка.
    /// </summary>
    /// <typeparam name="T"> Тип элементов списка. </typeparam>
    /// <param name="dice"> Кость, на основе которой делать случайный выбор. </param>
    /// <param name="list"> Список элементов, из которого выбирать элемент. </param>
    /// <returns> Случайный элемент из списка. </returns>
    public static T RollFromList<T>(this IDice dice, IReadOnlyList<T> list)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (list.Count == 1)
        {
            return list[0];
        }

        var rollIndex = dice.Roll(0, list.Count - 1);
        var item = list[rollIndex];
        return item;
    }

    /// <summary>
    /// Выбирает случайное значение из списка.
    /// </summary>
    /// <typeparam name="T"> Тип элементов списка. </typeparam>
    /// <param name="dice"> Кость, на основе которой делать случайный выбор. </param>
    /// <param name="list"> Список элементов, из которого выбирать элемент. </param>
    /// <param name="count">Количество выбранных значений. </param>
    /// <returns> Случайный элемент из списка. </returns>
    public static IEnumerable<T> RollFromList<T>(this IDice dice, IList<T> list, int count)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (list.Count < count)
        // ReSharper disable once LocalizableElement
        // Exception's messages shouldn't be localized.
        {
            throw new ArgumentException(
                $"The requested count {count} must be bigger or equal that list length {list.Count}.",
                nameof(count));
        }

        var openList = new List<T>(list);

        for (var i = 0; i < count; i++)
        {
            var rolledItem = dice.RollFromList(openList);

            yield return rolledItem;

            openList.Remove(rolledItem);
        }
    }

    public static IReadOnlyList<T> ShuffleList<T>(this IDice dice, IReadOnlyList<T> source)
    {
        var resultList = new List<T>();
        var openList = new List<T>(source);

        for (var i = 0; i < source.Count; i++)
        {
            var rolledItem = dice.RollFromList(openList);

            resultList.Add(rolledItem);
            openList.Remove(rolledItem);
        }

        return resultList;
    }
}