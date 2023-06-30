namespace Core.Combats.Effects;

public interface IDamageEffectModifier
{
    Range<int> Process(Range<int> damage);
}