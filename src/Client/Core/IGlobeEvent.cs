using System.Collections.Generic;

namespace Client.Core;

internal interface IGlobeEvent : IDisplayableJobExecutable
{
    IReadOnlyCollection<IJob> ExpirationConditions { get; }

    void Finish(Globe globe);
    void Start(Globe globe);
}