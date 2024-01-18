namespace Client.Core.Heroes.Factories;

internal interface IHeroFactory
{
    string ClassSid { get; }
    HeroState Create();
    CombatantGraphicsConfigBase GetGraphicsConfig();
}