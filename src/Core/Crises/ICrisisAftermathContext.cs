namespace Core.Crises;

public interface ICrisisAftermathContext
{
    void DamageHero(string heroClassSid, int damageAmount);
    IReadOnlyCollection<string> GetAvailableHeroes();
    IReadOnlyCollection<string> GetWoundedHeroes();
    void RestHero(string heroClassSid, int healAmount);

    event EventHandler<HeroStatChangedEventArgs>? HeroHpChanged;
}