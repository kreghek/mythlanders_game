using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IEventCatalog
    {
        IEnumerable<Event> Events { get; }

        EventNode GetDialogRoot(string sid);
    }

    internal interface IEventInitializer
    {
        void Init();
    }
}