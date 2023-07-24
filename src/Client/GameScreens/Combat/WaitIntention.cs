using Core.Combats;

namespace Client.GameScreens.Combat;

internal sealed class WaitIntention : IIntention
{
    public void Make(CombatEngineBase combatCore)
    {
        combatCore.Wait();
    }
}