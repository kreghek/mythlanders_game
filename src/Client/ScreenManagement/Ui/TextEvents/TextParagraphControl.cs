using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.DialogueOptionAftermath;
using Client.Core;
using Client.Engine;
using Client.GameScreens;
using Client.GameScreens.TextDialogue.Ui;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;
using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Client.ScreenManagement.Ui.TextEvents;

internal sealed class TextParagraphControl : ControlBase
{
    private readonly CampaignAftermathContext _aftermathContext;

    private readonly SpriteFont _displayNameFont;
    private readonly IReadOnlyCollection<IDialogueOptionAftermath<CampaignAftermathContext>> _paragraphAftermaths;
    private readonly string? _localizedSpeakerName;
    private readonly TextParagraphMessageControl _message;
    private readonly Vector2 _messageSize;
    private readonly IDialogueSpeaker _speaker;
    private readonly Vector2 _speakerDisplayNameSize;

    private bool _decorativeAftermathsAreExecuted;

    public TextParagraphControl(DialogueSpeech<ParagraphConditionContext, CampaignAftermathContext> eventTextParagraph,
        SoundEffect textSoundEffect, IDice dice, CampaignAftermathContext aftermathContext, IStoryState storyState) :
        base(UiThemeManager.UiContentStorage.GetControlBackgroundTexture())
    {
        _displayNameFont = UiThemeManager.UiContentStorage.GetMainFont();
        _aftermathContext = aftermathContext;
        _speaker = eventTextParagraph.Speaker;

        var speakerState = storyState.CharacterRelations.SingleOrDefault(x => x.Character.Equals(_speaker)) ??
                           new CharacterRelation(_speaker);

        _localizedSpeakerName = GetSpeakerDisplayName(speakerState);
        _message = new TextParagraphMessageControl(eventTextParagraph, textSoundEffect, dice,
            !DialogueSpeakers.Env.Equals(_speaker));
        _paragraphAftermaths = eventTextParagraph.Aftermaths.ToArray();

        _messageSize = _message.CalculateSize();
        _speakerDisplayNameSize = _localizedSpeakerName is not null
            ? _displayNameFont.MeasureString(_localizedSpeakerName)
            : Vector2.Zero;
    }

    public bool IsComplete => _message.IsComplete;

    public Vector2 CalculateSize()
    {
        var width = Math.Max(_messageSize.X, _speakerDisplayNameSize.X);
        var height = _speakerDisplayNameSize.Y + CONTENT_MARGIN + _messageSize.Y;

        return new Vector2(width + CONTENT_MARGIN * 2, height + CONTENT_MARGIN * 2);
    }

    public void FastComplete()
    {
        _message.FastComplete();
    }

    public void Update(GameTime gameTime)
    {
        if (!_decorativeAftermathsAreExecuted)
        {
            _decorativeAftermathsAreExecuted = true;

            foreach (var aftermath in _paragraphAftermaths)
            {
                aftermath.Apply(_aftermathContext);
            }
        }

        _message.Update(gameTime);
    }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
    {
        // Do nothing to draw transparent background.
        //base.DrawBackground(spriteBatch, color);
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
    {
        if (!DialogueSpeakers.Env.Equals(_speaker))
        {
            DrawSpeakerDisplayName(spriteBatch, clientRect.Location.ToVector2());
        }

        var messageTextPosition = clientRect.Location.ToVector2() + new Vector2(0, _speakerDisplayNameSize.Y) +
                                  new Vector2(0, CONTENT_MARGIN);
        _message.Rect = new Rectangle(messageTextPosition.ToPoint(), new Point(clientRect.Width, clientRect.Height));
        _message.Draw(spriteBatch);
    }

    private void DrawSpeakerDisplayName(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.DrawString(_displayNameFont, _localizedSpeakerName, position, Color.White);
    }

    private static string? GetSpeakerDisplayName(CharacterRelation characterRelation)
    {
        if (characterRelation.Character.Equals(DialogueSpeakers.Env))
        {
            return null;
        }

        return GameObjectHelper.GetLocalized(characterRelation);
    }
}