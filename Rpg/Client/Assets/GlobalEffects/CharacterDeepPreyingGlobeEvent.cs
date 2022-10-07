using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Client;

using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.GlobalEffects
{
    internal sealed class CharacterDeepPreyingGlobeEvent : IGlobeEvent
    {
        private readonly UnitName _name;
        private readonly GlobeRule _rule;

        public CharacterDeepPreyingGlobeEvent(UnitName name)
        {
            _name = name;

            var mappings = UnsortedHelpers.GetCharacterDisablingMap();

            var mapping = mappings.Single(x => x.Item1 == name);
            _rule = mapping.Item2;
        }

        public IReadOnlyList<GlobeRule> GetRules()
        {
            return new[] { _rule };
        }

        public bool IsActive => CombatsLeft > 0;

        public string Title => string.Format(GameObjectResources.CharacterDeepPreyingTitleTemplate, _name);
        public int CombatsLeft { get; private set; } = 5;

        public void Update()
        {
            CombatsLeft--;
        }

        public void Initialize(Globe globe)
        {
            var targetPlayerCharacter = globe.Player.GetAll().SingleOrDefault(x => x.UnitScheme.Name == _name);
            Debug.Assert(targetPlayerCharacter is not null,
                "Global events shoudn't be before the target character join the party.");

            targetPlayerCharacter.AddGlobalEffect(this);
        }
    }
}