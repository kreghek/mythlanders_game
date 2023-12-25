using System;

using Client.Core;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat;

internal sealed class BattlefieldInteractionContext : IBattlefieldInteractionContext
{
    private readonly ICombatantPositionProvider _combatantPositionProvider;
    private readonly CombatField _combatField;

    public BattlefieldInteractionContext(ICombatantPositionProvider combatantPositionProvider, CombatField combatField)
    {
        _combatantPositionProvider = combatantPositionProvider;
        _combatField = combatField;
    }

    private static (CombatFieldSide, CombatantPositionSide) GetTargetSide(ICombatant target, CombatField field)
    {
        try
        {
            var _ = field.HeroSide.GetCombatantCoords(target);
            return (field.HeroSide, CombatantPositionSide.Heroes);
        }
        catch (ArgumentException)
        {
            var _ = field.MonsterSide.GetCombatantCoords(target);
            return (field.MonsterSide, CombatantPositionSide.Monsters);
        }
    }

    public Rectangle GetArea(Team side)
    {
        if (side == Team.Cpu)
        {
            return new Rectangle(new Point(100 + 400, 100), new Point(200, 200));
        }

        return new Rectangle(new Point(100, 100), new Point(200, 200));
    }

    public Vector2 GetCombatantPosition(ICombatant combatant)
    {
        var side = GetTargetSide(combatant, _combatField);
        var coords = side.Item1.GetCombatantCoords(combatant);
        var position = _combatantPositionProvider.GetPosition(coords, side.Item2);

        return position;
    }
}