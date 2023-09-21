using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class PlayEffectDialogueOptionAftermath : IDecorativeEnvironmentAftermath
{
    private readonly string _effectSid;
    private readonly string _resourceName;

    public PlayEffectDialogueOptionAftermath(string effectSid, string resourceName)
    {
        _effectSid = effectSid;
        _resourceName = resourceName;
    }

    public void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.PlaySoundEffect(_effectSid, _resourceName);
    }
}