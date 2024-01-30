using Client.Assets.Catalogs.Dialogues;
using Client.Engine;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;
using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.TextDialogue.Ui;

internal sealed class TextParagraphMessageControl : ControlBase
{
    private readonly SpriteFont _font;
    private readonly bool _isCharacterSpeech;

    private readonly Speech _speech;
    
    public TextParagraphMessageControl(
        DialogueSpeech<ParagraphConditionContext, CampaignAftermathContext> eventTextFragment,
        SoundEffect textSoundEffect, IDice dice, bool isCharacterSpeech):
        base(UiThemeManager.UiContentStorage.GetControlBackgroundTexture())
    {
        _font = UiThemeManager.UiContentStorage.GetTitlesFont();

        var (text, _) = SpeechVisualizationHelper.PrepareLocalizedText(eventTextFragment.TextSid);
        _speech = new Speech(text, new SpeechSoundWrapper(textSoundEffect), new SpeechRandomProvider(dice));
        _isCharacterSpeech = isCharacterSpeech;
    }

    public bool IsComplete => _speech.IsComplete;

    public Vector2 CalculateSize()
    {
        var size = _font.MeasureString(_speech.FullText);
        return size + Vector2.One * (2 * CONTENT_MARGIN);
    }

    public void FastComplete()
    {
        _speech.MoveToCompletion();
    }

    public void Update(GameTime gameTime)
    {
        _speech.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    protected override Point CalcTextureOffset()
    {
        if (_isCharacterSpeech)
        {
            return ControlTextures.Speech;
        }

        return ControlTextures.Shadow;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
    {
        spriteBatch.DrawString(_font, _speech.GetCurrentText(),
            clientRect.Location.ToVector2() + Vector2.UnitX * 2,
            Color.SaddleBrown);
    }
}