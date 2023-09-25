using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class PlaySongDialogueOptionAftermath : DialogueOptionAftermathBase, IDecorativeEnvironmentAftermath
{
    private readonly string _resourceName;

    public PlaySongDialogueOptionAftermath(string resourceName)
    {
        _resourceName = resourceName;
        
        IsHidden = true;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.PlaySong(_resourceName);
    }
    
    protected override IReadOnlyList<string> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return ArraySegment<string>.Empty;
    }
}