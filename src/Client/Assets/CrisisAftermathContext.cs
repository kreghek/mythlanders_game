using System.Collections.Generic;
using System.Linq;

using Client.Core;

using Core.Crises;

namespace Client.Assets;

internal sealed class CrisisAftermathContext : ICrisisAftermathContext
{
    private Player _player;

    public CrisisAftermathContext(Player player)
    {
        _player = player;
    }

    public void DamageHero(string heroClassSid, int damageAmount)
    {
        var hero = _player.Heroes.Single(x => x.ClassSid == heroClassSid);
        hero.HitPoints.Consume(damageAmount);
    }

    public IReadOnlyCollection<string> GetAvailableHeroes()
    {
        return _player.Heroes.Where(x => x.HitPoints.Current > 0).Select(x => x.ClassSid).ToArray();
    }

    public IReadOnlyCollection<string> GetWoundedHeroes()
    {
        return _player.Heroes.Where(x => x.HitPoints.Current <= 0).Select(x => x.ClassSid).ToArray();
    }

    public void RestHero(string heroClassSid, int healAmount)
    {
        var hero = _player.Heroes.Single(x => x.ClassSid == heroClassSid);
        hero.HitPoints.Restore(healAmount);
    }
}