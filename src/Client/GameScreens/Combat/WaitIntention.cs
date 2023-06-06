using Core.Combats;

namespace Client.GameScreens.Combat;

internal sealed class WaitIntention : IIntention
{
    public void Make(CombatCore combatCore)
    {
        combatCore.Wait();
    }
}