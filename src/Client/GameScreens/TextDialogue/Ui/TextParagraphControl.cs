using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Client.Core;
using Client.Core.Dialogues;

using Core.Dices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;

namespace Client.GameScreens.TextDialogue.Ui;

internal sealed class TextParagraphControl : ControlBase
{
    private const int DISPLAY_NAME_HEIGHT = 32;

    private readonly SpriteFont _displayNameFont;
    private readonly IReadOnlyCollection<IDialogueEnvironmentEffect> _envCommands;
    private readonly IDialogueEnvironmentManager _envManager;
    private readonly string? _localizedSpeakerName;
    private readonly TextParagraphMessageControl _message;
    private readonly Vector2 _messageSize;
    private readonly UnitName _speaker;
    private readonly Vector2 _speakerDisplayNameSize;

    private bool _envCommandsExecuted;

    public TextParagraphControl(DialogueParagraph eventTextFragment,
        SoundEffect textSoundEffect, IDice dice, IDialogueEnvironmentManager envManager, IStoryState storyState)
    {
        _displayNameFont = UiThemeManager.UiContentStorage.GetMainFont();
        _envManager = envManager;
        _speaker = eventTextFragment.Speaker;

        var speakerState = storyState.CharacterRelations.SingleOrDefault(x => x.Name == _speaker) ??
                           new CharacterRelation(_speaker);

        _localizedSpeakerName = GetSpeakerDisplayName(speakerState);
        _message = new TextParagraphMessageControl(eventTextFragment, textSoundEffect, dice,
            _speaker != UnitName.Environment);
        _envCommands = eventTextFragment.EnvironmentEffects;

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
        if (!_envCommandsExecuted)
        {
            _envCommandsExecuted = true;

            foreach (var envCommand in _envCommands)
            {
                envCommand.Execute(_envManager);
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
        if (_speaker != UnitName.Environment)
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
        if (characterRelation.Name == UnitName.Environment)
        {
            return null;
        }

        if (characterRelation.Name == UnitName.Undefined)
        {
            Debug.Fail("Speaker is undefined.");
            return null;
        }

        return GameObjectHelper.GetLocalized(characterRelation);
    }
}