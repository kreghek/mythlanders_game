namespace Rpg.Client.Core
{
    internal sealed class AddPlayerCharacterOptionAftermath : IOptionAftermath
    {
        private readonly UnitScheme _scheme;

        public AddPlayerCharacterOptionAftermath(UnitScheme scheme)
        {
            _scheme = scheme;
        }

        public void Apply(IEventContext dialogContext)
        {
            const int DEFAULT_LEVEL = 1;
            var unit = new Unit(_scheme, DEFAULT_LEVEL)
            {
                IsPlayerControlled = true
            };
            dialogContext.AddNewCharacter(unit);
        }
    }

    internal sealed class UnitDeepPreyingOptionAftermath : IOptionAftermath
    {
        private readonly UnitName _name;

        public UnitDeepPreyingOptionAftermath(UnitName name)
        {
            _name = name;
        }

        public void Apply(IEventContext dialogContext)
        {
            var globalEvent = new CharacterDeepPreyingGlobeEvent(_name);

            dialogContext.AddNewGlobalEvent(globalEvent);
        }
    }
}