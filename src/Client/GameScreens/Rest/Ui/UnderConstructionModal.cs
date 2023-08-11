using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Rest.Ui;

internal sealed class UnderConstructionModal : ModalDialogBase
{
    private readonly Texture2D _underConstructionTexture;
    private readonly IUiContentStorage _uiContentStorage;

    public UnderConstructionModal(Texture2D underConstructionTexture,
        IUiContentStorage uiContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer) : base(uiContentStorage,
        resolutionIndependentRenderer)
    {
        _underConstructionTexture = underConstructionTexture;
        _uiContentStorage = uiContentStorage;
    }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_underConstructionTexture, ContentRect, Color.White);

        var size = _uiContentStorage.GetTitlesFont().MeasureString(UiResource.UnderConstructionModalContent);
        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), UiResource.UnderConstructionModalContent,
            ContentRect.Center.ToVector2() - size / 2,
            Color.White);
    }
}