namespace Core.Crises;

public interface ICrisisAftermathContext
{
    IReadOnlyCollection<string> GetAvailableHeroes();
    IReadOnlyCollection<string> GetWoundedHeroes();
    void DamageHero(string heroClassSid, int damageAmount);
    void RestHero(string heroClassSid, int healAmount);

    event EventHandler<HeroStatChangedEventArgs>? HeroHpChanged;
}