using Client.Core;
using Client.Engine;
using Client.GameScreens.Common;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Tutorial;

internal class CombatEgyptianTutorial1PageDrawer : TutorialPageDrawerBase
{
    private readonly ControlBase _content;

    public CombatEgyptianTutorial1PageDrawer(IUiContentStorage uiContentStorage) : base(uiContentStorage)
    {
        var elements = new[]
        {
            CreateText(uiContentStorage, UiResource.CombatEgyptianTutorial1Paragraph1)
        };
        
        _content = new VerticalStackPanel(uiContentStorage.GetControlBackgroundTexture(), ControlTextures.Transparent,
            elements);
    }

    private static ControlBase CreateText(IUiContentStorage uiContentStorage, string text)
    {
        return new RichText(uiContentStorage.GetControlBackgroundTexture(),
            ControlTextures.Transparent,
            uiContentStorage.GetMainFont(), 
            _ => Color.White,
            () => StringHelper.RichLineBreaking(text, 65));
    }

    public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        _content.Rect = contentRect;
        _content.Draw(spriteBatch);
    }
}