namespace Core.Combats;

public class CombatField
{
    public CombatFieldSide HeroSide { get; }
    public CombatFieldSide MonsterSide { get; }

    public CombatField()
    {
        HeroSide = new CombatFieldSide();
        MonsterSide = new CombatFieldSide();
    }
}

public sealed class CombatFieldSide
{
    private readonly Matrix<FormationSlot> _matrix;

    public const int COLUMN_COUNT = 2;
    public const int LINE_COUNT = 3;

    public CombatFieldSide()
    {
        _matrix = new Matrix<FormationSlot>(COLUMN_COUNT, LINE_COUNT);

        for (var columnIndex = 0; columnIndex < COLUMN_COUNT; columnIndex++)
        {
            for (var lineIndex = 0; lineIndex < LINE_COUNT; lineIndex++)
            {
                _matrix[columnIndex, lineIndex] = new FormationSlot(columnIndex, lineIndex);
            }
        }
    }

    public int ColumnCount => _matrix.Width;

    public int LineCount => _matrix.Height;

    public FormationSlot this[FieldCoords coords]
    {
        get { return _matrix[coords.ColumentIndex, coords.LineIndex]; }
        set { _matrix[coords.ColumentIndex, coords.LineIndex] = value; }
    }
}

public sealed record FieldCoords(int ColumentIndex, int LineIndex);

/// <summary>
/// ������� ��������.
/// </summary>
/// <typeparam name="T"> ��� �������� �������. </typeparam>
public sealed class Matrix<T>
{
    /// <summary>
    /// ����������� ������� ��������.
    /// </summary>
    /// <param name="items"> ��������� ������, ������� ����� ������ � ������ �������. </param>
    /// <param name="width"> ������ �������. ������ ��������������� �������� �������. </param>
    /// <param name="height"> ������ �������. ������ ��������������� �������� �������. </param>
    public Matrix(T[,] items, int width, int height)
    {
        CheckArguments(width, height);

        Items = items ?? throw new ArgumentNullException(nameof(items));

        Width = width;
        Height = height;
    }

    /// <summary>
    /// ����������� ������� ��������.
    /// </summary>
    /// <param name="width"> ������ �������. ������ ��������������� �������� �������. </param>
    /// <param name="height"> ������ �������. ������ ��������������� �������� �������. </param>
    public Matrix(int width, int height)
    {
        CheckArguments(width, height);
        Width = width;
        Height = height;

        Items = new T[width, height];
    }

    /// <summary>
    /// ������ �������.
    /// </summary>
    public int Height { get; }

    public T this[int x, int y]
    {
        get => Items[x, y];
        set => Items[x, y] = value;
    }

    /// <summary>
    /// �������� �������.
    /// </summary>
    public T[,] Items { get; }

    /// <summary>
    /// ������ �������.
    /// </summary>
    public int Width { get; }

    public bool IsIn(int x, int y)
    {
        if (x >= Width || y >= Height)
        {
            return false;
        }

        if (x < 0 || y < 0)
        {
            return false;
        }

        return true;
    }

    private static void CheckArguments(int width, int height)
    {
        if (width <= 0)
        {
            throw new ArgumentException("������ ������ ���� ������ 0.", nameof(width));
        }

        if (height <= 0)
        {
            throw new ArgumentException("������ ������ ���� ������ 0.", nameof(height));
        }
    }
}