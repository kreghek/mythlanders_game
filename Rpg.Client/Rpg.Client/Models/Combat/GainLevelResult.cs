using Rpg.Client.Core;

namespace Rpg.Client.Models.Combat
{
    internal sealed record GainLevelResult
    {
        public bool IsLevelUp => StartXp + XpAmount >= XpToLevelup;
        public int StartXp { get; init; }
        public Unit Unit { get; init; }
        public int XpAmount { get; init; }
        public int XpToLevelup { get; init; }
    }
}