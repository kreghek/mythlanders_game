namespace Rpg.Client.GameScreens.Event.Ui
{
    internal interface ISpeechSoundWrapper
    {
        public double Duration { get; }
        public void Play();
    }
}