using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rpg.Client.Core.GlobalEvents
{
    internal sealed class CharacterDeepPreyingGlobeEvent : IGlobeEvent
    {
        private readonly UnitName _name;
        private readonly GlobeRule _rule;

        public CharacterDeepPreyingGlobeEvent(UnitName name)
        {
            _name = name;

            if (_name is UnitName.Berimir)
            {
                _rule = GlobeRule.DisableBerimir;
            }
            else
            {
                Debug.Fail($"{name} is unknown character name.");
                _rule = GlobeRule.DisableBerimir;
            }
        }

        public IReadOnlyList<GlobeRule> GetRules()
        {
            return new[] { _rule };
        }

        public bool IsActive => CombatsLeft > 0;

        public string Title => $"{_name} is preying";
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