using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class ActivateStoryPointOptionAftermath : IDialogueOptionAftermath<AftermathContext>
{
    private readonly string _storyPointSid;

    public ActivateStoryPointOptionAftermath(string storyPointSid)
    {
        _storyPointSid = storyPointSid;
    }

    public void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.AddStoryPoint(_storyPointSid);
    }
}