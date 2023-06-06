using System.Diagnostics.CodeAnalysis;

namespace Core.Props;

public sealed class Resource : PropBase
{
    private int _count;

    [ExcludeFromCodeCoverage]
    public Resource(IPropScheme scheme, int count) : base(scheme)
    {
        if (count <= 0) throw new ArgumentException("Resources cannot be 0 or less.", nameof(count));

        _count = count;
    }

    /// <summary>
    /// Number of resource units.
    /// </summary>
    public int Count
    {
        get => _count;
        set
        {
            if (value <= 0) throw new ArgumentException("Value cannot be less than or equal to 0.");

            _count = value;
            DoChange();
        }
    }

    /// <summary>
    /// Splits the current resource drain and creates a new instance with the specified amount.
    /// </summary>
    /// <param name="count"> The number of resource units in the new heap. </param>
    /// <returns> An instance of a detached resource heap. </returns>
    public Resource CreateHeap(int count)
    {
        var resource2 = new Resource(Scheme, count);
        Count -= count;
        return resource2;
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"{Scheme} x {Count}";
    }

    [ExcludeFromCodeCoverage]
    private void DoChange()
    {
        Changed?.Invoke(this, new EventArgs());
    }

    public event EventHandler<EventArgs>? Changed;
}