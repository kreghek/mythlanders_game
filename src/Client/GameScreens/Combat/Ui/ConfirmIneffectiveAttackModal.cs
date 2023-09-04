using System;

using Client.Engine;

using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class ConfirmIneffectiveAttackModal: ModalDialogBase
{
    private readonly ResourceTextButton _confirmButton;

    public ConfirmIneffectiveAttackModal(IUiContentStorage uiContentStorage, IResolutionIndependentRenderer resolutionIndependentRenderer) : base(uiContentStorage, resolutionIndependentRenderer)
    {
        _confirmButton = new ResourceTextButton(nameof(UiResource.CloseButtonTitle));
        _confirmButton.OnClick += ConfirmButton_OnClick;
    }

    private void ConfirmButton_OnClick(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        
    }
}