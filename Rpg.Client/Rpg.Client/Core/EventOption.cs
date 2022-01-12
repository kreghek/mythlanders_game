using System.Collections.Generic;
using System.Diagnostics;

namespace Rpg.Client.Core
{
    internal sealed class EventOption
    {
        public IOptionAftermath? Aftermath { get; init; }
        public bool IsEnd { get; init; }
        public EventNode Next { get; init; }
        public string TextSid { get; init; }
    }

    internal interface IGlobeEvent
    {
        IReadOnlyList<GlobeRule> GetRules();
        bool IsActive { get; }
        void Update();
    }
    
    internal sealed class CharacterDeepPreyingGlobeEvent: IGlobeEvent
    {
        private readonly UnitName _name;
        private readonly GlobeRule _rule;
        private int _counter = 5;

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

        public bool IsActive => _counter > 0;

        public void Update()
        {
            _counter--;
        }
    }


    internal enum GlobeRule
    {
        DisableBerimir
    }
}