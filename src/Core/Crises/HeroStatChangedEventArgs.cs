namespace Core.Crises;

public sealed class HeroStatChangedEventArgs : EventArgs
{
    public HeroStatChangedEventArgs(string hero, int amount)
    {
        Hero = hero;
        Amount = amount;
    }

    public string Hero { get; }
    public int Amount { get; }
}