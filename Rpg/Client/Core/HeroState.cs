using Core.Combats;

namespace Client.Core;

internal sealed class HeroState
{
    public string ClassSid { get; set; }
    public IStatValue HitPoints { get; set; }
}