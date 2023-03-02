namespace Core.Combats;

public class StatValue : IStatValue
{
    private readonly IList<IUnitStatModifier> _modifiers;

    public StatValue(int baseValue)
    {
        Base = baseValue;
        Current = Base;
        _modifiers = new List<IUnitStatModifier>();
    }

    private int Base { get; set; }

    event EventHandler IStatValue.ModifierAdded
    {
        add => throw new NotImplementedException();

        remove => throw new NotImplementedException();
    }

    public int ActualMax => Base + _modifiers.Sum(x => x.Value);

    public int Current { get; private set; }

    public void AddModifier(IUnitStatModifier modifier)
    {
        _modifiers.Add(modifier);
        if (Current > Base) Current = Base;

        ModifierAdded?.Invoke(this, new EventArgs());
    }

    public void ChangeBase(int newBase)
    {
        Base = newBase;
        Current = newBase;
    }

    public void Consume(int value)
    {
        Current -= value;

        if (Current < 0) Current = 0;
    }

    public void CurrentChange(int newCurrent)
    {
        Current = Math.Min(newCurrent, ActualMax);
    }

    public void Restore(int value)
    {
        Current += value;

        if (Current > Base) Current = Base;
    }

    public void RemoveModifier(IUnitStatModifier modifier)
    {
        _modifiers.Remove(modifier);
    }

    public event EventHandler? ModifierAdded;
}