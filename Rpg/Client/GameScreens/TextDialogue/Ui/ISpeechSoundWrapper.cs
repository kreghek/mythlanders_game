namespace Rpg.Client.GameScreens.Speech.Ui
{
    internal interface ISpeechSoundWrapper
    {
        public double Duration { get; }
        public void Play();
    }
}