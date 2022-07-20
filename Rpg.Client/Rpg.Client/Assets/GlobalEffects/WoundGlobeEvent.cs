using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rpg.Client.Assets.SkillEffects;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.GlobalEffects
{
    internal sealed class WoundGlobeEvent : IGlobeEvent
    {
        private readonly UnitName _unitName;
        private readonly float _penalty;

        public WoundGlobeEvent(UnitName unitName, float penalty)
        {
            _unitName = unitName;
            _penalty = penalty;
        }

        public bool IsActive => CombatsLeft > 0;

        public string Title => $"Рана {_unitName} -{_penalty * 100}% HP";
        public int CombatsLeft { get; private set; } = 2;

        public void Update()
        {
            CombatsLeft--;
        }

        public void Initialize(Globe globe)
        {
            var targetPlayerCharacter = globe.Player.GetAll().SingleOrDefault(x => x.UnitScheme.Name == _unitName);
            Debug.Assert(targetPlayerCharacter is not null,
                "Global events shoudn't be before the target character join the party.");

            targetPlayerCharacter.AddGlobalEffect(this);
        }

        public IReadOnlyList<GlobeRule> GetRules()
        {
            return Array.Empty<GlobeRule>();
        }

        public IReadOnlyCollection<EffectRule> CreateCombatBeginningEffects()
        {
            return new[]
            {
                new EffectRule
                {
                    Direction = SkillDirection.Self,
                    EffectCreator = new EffectCreator(u =>
                    {
                        var lifetime = new UnitBoundEffectLifetime(u);
                        return new HitPointModifyEffect(u, lifetime, -_penalty)
                        {
                            Visualization = EffectVisualizations.HitPointsDown
                        };
                    })
                }
            };
        }
    }
}
