﻿using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.DialogueOptionAftermath
{
    internal sealed class AddHeroOptionAftermath : IOptionAftermath
    {
        private readonly UnitScheme _scheme;

        public AddHeroOptionAftermath(UnitScheme scheme)
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
}