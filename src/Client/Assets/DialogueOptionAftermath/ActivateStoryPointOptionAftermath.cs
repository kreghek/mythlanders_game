using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class ActivateStoryPointOptionAftermath : DialogueOptionAftermathBase
{
    private readonly string _storyPointSid;

    public ActivateStoryPointOptionAftermath(string storyPointSid)
    {
        _storyPointSid = storyPointSid;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.AddStoryPoint(_storyPointSid);
    }

    protected override IReadOnlyList<string> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return new[] { _storyPointSid };
    }
}