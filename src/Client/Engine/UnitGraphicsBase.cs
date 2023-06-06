using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.GameScreens;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

internal enum OutlineMode
{
    None,
    AvailableEnemyTarget,
    SelectedEnemyTarget,
    SelectedAlyTarget
}

internal abstract class UnitGraphicsBase
{
    private const int FRAME_WIDTH = 256;
    private const int FRAME_HEIGHT = 128;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly UnitGraphicsConfigBase _graphicsConfig;

    private readonly IDictionary<PredefinedAnimationSid, IAnimationFrameSet> _predefinedAnimationFrameSets;
    private readonly Sprite _selectedMarker;

    private IAnimationFrameSet _currentAnimationFrameSet = null!;
    private Sprite _graphics;
    private Sprite[] _outlines;
    protected Vector2 _position;

    private Sprite[] _sprites;

    public UnitGraphicsBase(UnitName spriteSheetId, UnitGraphicsConfigBase graphicsConfig, bool isNormalOrientation,
        Vector2 position, GameObjectContentStorage gameObjectContentStorage)
    {
        _graphicsConfig = graphicsConfig;
        _position = position;
        _gameObjectContentStorage = gameObjectContentStorage;

        _selectedMarker = new Sprite(gameObjectContentStorage.GetCombatUnitMarker())
        {
            Origin = new Vector2(0.5f, 0.75f),
            SourceRectangle = new Rectangle(0, 0, 128, 32)
        };

        _predefinedAnimationFrameSets = graphicsConfig.GetPredefinedAnimations();
        InitializeSprites(spriteSheetId, isNormalOrientation);

        PlayAnimation(PredefinedAnimationSid.Idle);
    }

    public OutlineMode OutlineMode { get; set; }

    public SpriteContainer Root { get; private set; }

    public bool ShowActiveMarker { get; set; }

    public void Draw(SpriteBatch spriteBatch)
    {
        Root.Draw(spriteBatch);
    }

    public IAnimationFrameSet GetAnimationInfo(PredefinedAnimationSid sid)
    {
        return _predefinedAnimationFrameSets[sid];
    }

    public void PlayAnimation(IAnimationFrameSet animation)
    {
        if (_currentAnimationFrameSet == animation)
        {
            // Do nothing. Just continue current animation.
            return;
        }

        _currentAnimationFrameSet = animation;
        _currentAnimationFrameSet.Reset();
    }

    public void PlayAnimation(PredefinedAnimationSid sid)
    {
        var animation = _predefinedAnimationFrameSets[sid];
        PlayAnimation(animation);
    }

    public void SwitchSourceUnit(UnitName spriteSheetId, bool isNormalOriented)
    {
        InitializeSprites(spriteSheetId, isNormalOriented);
    }

    public void Update(GameTime gameTime)
    {
        HandleSelectionMarker();

        UpdateAnimation(gameTime);

        foreach (var outline in _outlines)
        {
            outline.Visible = OutlineMode != OutlineMode.None;
        }
    }

    protected void InitializeSprites(UnitName spriteSheetId, bool isPlayerSide)
    {
        if (Root is not null)
        {
            Root.RemoveChild(_selectedMarker);
        }

        Root = new SpriteContainer
        {
            Position = _position,
            FlipX = !isPlayerSide
        };

        var shadow = new Sprite(_gameObjectContentStorage.GetUnitShadow())
        {
            Origin = new Vector2(0.5f, 0.5f),
            Color = Color.Lerp(Color.Black, Color.Transparent, 0.5f),
            Position = _graphicsConfig.ShadowOrigin
        };
        Root.AddChild(shadow);

        Root.AddChild(_selectedMarker);

        _graphics = CreateSprite(spriteSheetId, Vector2.Zero, Color.White);

        var outlineCount = 4;
        var outlineLength = 2;
        _outlines = Enumerable.Range(0, outlineCount).Select(x => CreateSprite(
            spriteSheetId,
            new Vector2((float)Math.Cos(Math.PI * 2 / outlineCount * x),
                (float)Math.Sin(Math.PI * 2 / outlineCount * x)) * outlineLength,
            Color.Red)
        ).ToArray();

        var sprites = new List<Sprite>();

        foreach (var outline in _outlines)
        {
            outline.Visible = false;
            sprites.Add(outline);
            Root.AddChild(outline);
        }

        Root.AddChild(_graphics);
        sprites.Add(_graphics);

        _sprites = sprites.ToArray();
    }

    private Sprite CreateSprite(UnitName spriteSheetId, Vector2 offset, Color baseColor)
    {
        return new Sprite(_gameObjectContentStorage.GetUnitGraphics(spriteSheetId))
        {
            Origin = new Vector2(0.5f, 0.875f),
            SourceRectangle = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT),
            Position = new Vector2(FRAME_WIDTH * 0.25f, 0) + offset,
            Color = baseColor
        };
    }

    private void HandleSelectionMarker()
    {
        var isMarkerDisplayed = _currentAnimationFrameSet.IsIdle && ShowActiveMarker;
        _selectedMarker.Visible = isMarkerDisplayed;
    }

    private void UpdateAnimation(GameTime gameTime)
    {
        _currentAnimationFrameSet.Update(gameTime);

        foreach (var sprite in _sprites)
        {
            sprite.SourceRectangle = _currentAnimationFrameSet.GetFrameRect();
        }
    }
}