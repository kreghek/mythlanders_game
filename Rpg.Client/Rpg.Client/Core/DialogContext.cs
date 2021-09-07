using System.Collections.Generic;

namespace Rpg.Client.Core
{
    public sealed class DialogContext : IDialogContext
    {
        private readonly Globe _globe;

        public DialogContext(Globe globe)
        {
            _globe = globe;
        }

        public void AddNewCharacter(Unit unit)
        {
            var units = new List<Unit>(_globe.Player.Group.Units);

            units.Add(unit);

            _globe.Player.Group.Units = units;
        }
    }
}