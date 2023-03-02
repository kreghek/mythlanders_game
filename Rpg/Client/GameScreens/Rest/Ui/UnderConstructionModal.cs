using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.Rest.Ui;

internal sealed class UnderConstructionModal : ModalDialogBase
{
    private readonly IUiContentStorage _uiContentStorage;

    public UnderConstructionModal(IUiContentStorage uiContentStorage,
        ResolutionIndependentRenderer resolutionIndependentRenderer) : base(uiContentStorage,
        resolutionIndependentRenderer)
    {
        _uiContentStorage = uiContentStorage;
    }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), "Under construction", ContentRect.Center.ToVector2(),
            Color.White);
    }
}