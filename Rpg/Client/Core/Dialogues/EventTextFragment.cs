using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Client.Core.Dialogues;

internal sealed class EventTextFragment
{
    public UnitName Speaker { get; }
    public string TextSid { get; }

    public EventTextFragment(UnitName speaker, string textSid)
    {
        TextSid = textSid;
        Speaker = speaker;

        EnvironmentCommands = Array.Empty<IDialogueEventTextFragmentEnvironmentCommand>();
    }

    public IReadOnlyCollection<IDialogueEventTextFragmentEnvironmentCommand> EnvironmentCommands { get; init; }
}