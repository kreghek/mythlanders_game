using CombatDicesTeam.Dices;

namespace Client.GameScreens.TextDialogue.Ui;

internal sealed class SpeechRandomProvider : ISpeechRandomProvider
{
    private readonly IDice _dice;

    public SpeechRandomProvider(IDice dice)
    {
        _dice = dice;
    }

    public float RollPlayingSoundOnSymbol()
    {
        return _dice.Roll(100) * 0.01f;
    }
}