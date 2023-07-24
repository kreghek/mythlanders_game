namespace Core.Combats.Effects;
using Core.Utils;

public interface IDamageEffectModifier
{
    Range<int> Process(Range<int> damage);
}