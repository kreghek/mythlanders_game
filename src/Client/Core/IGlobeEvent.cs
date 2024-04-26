using System.Collections.Generic;

namespace Client.Core;

internal interface IGlobeEvent
{
    string TitleSid { get; }

    void Start(Globe globe);

    void Finish(Globe globe);

    IReadOnlyCollection<IJob> ExpirationConditions { get; }
}