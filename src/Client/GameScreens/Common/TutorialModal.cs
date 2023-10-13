using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common;

internal class TutorialModal : ModalDialogBase
{
    private readonly TutorialPageDrawerBase _pageDrawer;
    private readonly ResourceTextButton _skipTutorialButton;

    public TutorialModal(TutorialPageDrawerBase pageDrawer, IUiContentStorage uiContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer, Player player) : base(uiContentStorage,
        resolutionIndependentRenderer)
    {
        _pageDrawer = pageDrawer;

        _skipTutorialButton = new ResourceTextButton(nameof(UiResource.SkipTutorialButtonTitle));
        _skipTutorialButton.OnClick += (_, _) =>
        {
            player.AddPlayerAbility(PlayerAbility.SkipTutorials);
            Close();
        };
    }


    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        var pageRect = new Rectangle(ContentRect.Location, ContentRect.Size - new Point(0, 20));
        _pageDrawer.Draw(spriteBatch, pageRect);

        _skipTutorialButton.Rect = new Rectangle(ContentRect.X, pageRect.Bottom, 100, 20);
        _skipTutorialButton.Draw(spriteBatch);
    }

    protected override void UpdateContent(GameTime gameTime,
        IScreenProjection screenProjection)
    {
        base.UpdateContent(gameTime, screenProjection);

        _skipTutorialButton.Update(screenProjection);
    }
}