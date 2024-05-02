﻿using Client.Core;
using Client.Engine;
using Client.GameScreens.Common;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Tutorial;

internal class CombatSlavicTutorial1PageDrawer : TutorialPageDrawerBase
{
    private readonly ControlBase _content;

    public CombatSlavicTutorial1PageDrawer(IUiContentStorage uiContentStorage, Texture2D texture2D) : base(uiContentStorage)
    {
        var elements = new[]
        {
            CreateText(uiContentStorage, UiResource.CombatSlavicTutorial1Paragraph1),
            new Image(texture2D, texture2D.Bounds, uiContentStorage.GetControlBackgroundTexture(), ControlTextures.Transparent),
            CreateText(uiContentStorage, UiResource.CombatSlavicTutorial1Paragraph2),
            CreateText(uiContentStorage, UiResource.CombatSlavicTutorial1Paragraph3),
            CreateText(uiContentStorage, UiResource.CombatSlavicTutorial1Paragraph4),
            CreateText(uiContentStorage, UiResource.CombatSlavicTutorial1Paragraph5)
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