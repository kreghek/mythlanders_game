﻿using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IEventCatalog
    {
        IEnumerable<Event> Events { get; }
    }
}