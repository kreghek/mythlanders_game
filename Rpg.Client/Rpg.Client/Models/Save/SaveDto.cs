using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Models.Save
{
    internal record SaveDto
    {
        public IList<Core.Combat> Combats { get; init; }

        public PlayerParty Player { get; init; }
    }

    internal record PlayerParty
    {
        public GroupUnits Group { get; init; }

        public GroupUnits Pool { get; init; }
    }

    internal record GroupUnits
    {
        public IEnumerable<UnitDto> Units { get; init; }
    }

    internal record UnitDto
    {
        public int Hp { get; init; }

        public int Level { get; init; }
        public string SchemeName { get; init; }

        public IEnumerable<string> SkillSids { get; init; }

        public int Xp { get; init; }
    }
}