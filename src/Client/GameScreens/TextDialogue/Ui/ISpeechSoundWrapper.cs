namespace Client.GameScreens.TextDialogue.Ui;

internal interface ISpeechSoundWrapper
{
    public double Duration { get; }
    public void Play();
}