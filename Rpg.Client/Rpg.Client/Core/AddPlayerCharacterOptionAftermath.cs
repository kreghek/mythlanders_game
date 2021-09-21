﻿namespace Rpg.Client.Core
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
            //TODO Adjust level to average party level.
            const int DEFAULT_LEVEL = 1;
            var unit = new Unit(_scheme, DEFAULT_LEVEL)
            {
                IsPlayerControlled = true,
                EquipmentLevel = 1
            };
            dialogContext.AddNewCharacter(unit);
        }
    }
}