using System;

using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class ConfirmIneffectiveAttackModal : ModalDialogBase
{
    private readonly IUiContentStorage _uiContentStorage;
    private readonly SoundEffect _alertSoundEffect;
    private readonly Action _confirmDelegate;
    private readonly ResourceTextButton _confirmButton;
    private readonly ResourceTextButton _rejectButton;

    public ConfirmIneffectiveAttackModal(IUiContentStorage uiContentStorage,
        SoundEffect alertSoundEffect,
        IResolutionIndependentRenderer resolutionIndependentRenderer,
        Action confirmDelegate) : base(uiContentStorage, resolutionIndependentRenderer)
    {
        _uiContentStorage = uiContentStorage;
        _alertSoundEffect = alertSoundEffect;
        _confirmDelegate = confirmDelegate;
        _rejectButton = new ResourceTextButton(nameof(UiResource.RejectButtonTitle));
        _rejectButton.OnClick += RejectButton_OnClick;

        _confirmButton = new ResourceTextButton(nameof(UiResource.ConfirmButtonTitle));
        _confirmButton.OnClick += ConfirmButton_OnClick;

        _alertSoundEffect.CreateInstance().Play();
    }

    private void RejectButton_OnClick(object? sender, EventArgs e)
    {
        Close();
    }

    private void ConfirmButton_OnClick(object? sender, EventArgs e)
    {
        Close();
        _confirmDelegate();
    }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        var warningFont = _uiContentStorage.GetTitlesFont();

        var sourceWarningText = UiResource.ConfirmIneffectiveAttackText;

        var wrappedText = StringHelper.LineBreaking(sourceWarningText, 30);

        var warningTextSize = warningFont.MeasureString(wrappedText);

        var textPosition = AlignTextToParentCenter(warningTextSize);

        spriteBatch.DrawString(warningFont,
            wrappedText,
            textPosition, TestamentColors.MainAncient);

        _rejectButton.Rect = new Rectangle(ContentRect.Left + ControlBase.CONTENT_MARGIN,
            ContentRect.Bottom - (20 + ControlBase.CONTENT_MARGIN), 100, 20);

        _rejectButton.Draw(spriteBatch);


        _confirmButton.Rect = new Rectangle(ContentRect.Right - (ControlBase.CONTENT_MARGIN + 100),
            ContentRect.Bottom - (20 + ControlBase.CONTENT_MARGIN), 100, 20);

        _confirmButton.Draw(spriteBatch);
    }

    private Vector2 AlignTextToParentCenter(Vector2 warningTextSize)
    {
        return (ContentRect.Size.ToVector2() - warningTextSize) * 0.5f + ContentRect.Location.ToVector2();
    }
}