using Client.Core;
using Client.Engine;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Title;

internal sealed class DemoLimitsModal : ModalDialogBase
{
    private SpriteFont _textFont;

    public DemoLimitsModal(IUiContentStorage uiContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer) : base(uiContentStorage,
        resolutionIndependentRenderer)
    {
        _textFont = uiContentStorage.GetMainFont();
    }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_textFont, StringHelper.LineBreaking(UiResource.DemoLimitsText, 70), ContentRect.Location.ToVector2() + new Vector2(ControlBase.CONTENT_MARGIN * 2), MythlandersColors.MainSciFi);
    }
}
