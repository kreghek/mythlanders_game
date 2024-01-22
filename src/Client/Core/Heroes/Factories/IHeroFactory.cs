namespace Client.Core.Heroes.Factories;

internal interface IHeroFactory
{
    HeroState Create();
}