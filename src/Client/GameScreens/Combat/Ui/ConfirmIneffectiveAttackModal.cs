using System;

using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class ConfirmIneffectiveAttackModal: ModalDialogBase
{
    private readonly IUiContentStorage _uiContentStorage;
    private readonly Action _confirmDelegate;
    private readonly ResourceTextButton _confirmButton;
    private readonly ResourceTextButton _rejectButton;

    public ConfirmIneffectiveAttackModal(IUiContentStorage uiContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer,
        Action confirmDelegate) : base(uiContentStorage, resolutionIndependentRenderer)
    {
        _uiContentStorage = uiContentStorage;
        _confirmDelegate = confirmDelegate;
        _rejectButton = new ResourceTextButton(nameof(UiResource.RejectButtonTitle));
        _rejectButton.OnClick += RejectButton_OnClick;
        
        _confirmButton = new ResourceTextButton(nameof(UiResource.ConfirmButtonTitle));
        _confirmButton.OnClick += ConfirmButton_OnClick;
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
        // TODO Play alert sound

        var warningFont = _uiContentStorage.GetTitlesFont();
        
        spriteBatch.DrawString(warningFont,
            UiResource.ConfirmIneffectiveAttackText,
            ContentRect.Location.ToVector2(), TestamentColors.MainAncient);

        _rejectButton.Rect = new Rectangle(ContentRect.Left + ControlBase.CONTENT_MARGIN,
            ContentRect.Top - (20 + ControlBase.CONTENT_MARGIN), 100, 20);
        
        _rejectButton.Rect = new Rectangle(ContentRect.Right - (ControlBase.CONTENT_MARGIN - 100),
            ContentRect.Top - (20 + ControlBase.CONTENT_MARGIN), 100, 20);
    }
}