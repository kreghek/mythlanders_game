using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.GameScreens;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.SceneGraphs;
using MonoGame.Extended.TextureAtlases;

using MonoSprite = MonoGame.Extended.Sprites.Sprite;

namespace Client.Engine;

internal abstract class UnitGraphicsBase
{
    private const int FRAME_WIDTH = 256;
    private const int FRAME_HEIGHT = 128;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly CombatantGraphicsConfigBase _graphicsConfig;
    private readonly Texture2D _mainTexture;

    private readonly IDictionary<PredefinedAnimationSid, IAnimationFrameSet> _predefinedAnimationFrameSets;
    private readonly (SceneNode, MonoSprite)[] _selectedMarkers;

    private MonoSprite[] _animatedSprites;
    private IAnimationFrameSet _currentAnimationFrameSet = null!;
    private (SceneNode, MonoSprite) _mainGraphics;
    private (SceneNode, MonoSprite)[] _outlines;

    private double _selectedMarkerCounter;
    private SceneNode _shadowNode;

    public UnitGraphicsBase(UnitName spriteSheetId, CombatantGraphicsConfigBase graphicsConfig,
        bool isNormalOrientation,
        Vector2 startPposition, GameObjectContentStorage gameObjectContentStorage)
    {
        _graphicsConfig = graphicsConfig;
        _gameObjectContentStorage = gameObjectContentStorage;
        _selectedMarkers = CreateSelectionMarkers(gameObjectContentStorage);
        _mainTexture = _gameObjectContentStorage.GetUnitGraphics(spriteSheetId);

        _predefinedAnimationFrameSets = graphicsConfig.GetPredefinedAnimations();
        InitializeSprites(spriteSheetId, startPposition, isNormalOrientation);

        PlayAnimation(PredefinedAnimationSid.Idle);
    }

    public OutlineMode OutlineMode { get; set; }

    public SceneGraph Root { get; private set; }

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
        PlayAnimation(_predefinedAnimationFrameSets.TryGetValue(sid, out var animation)
            ? animation
            : _predefinedAnimationFrameSets[sid]);
    }

    public void Update(GameTime gameTime)
    {
        HandleSelectionMarker(gameTime);

        UpdateAnimation(gameTime);

        foreach (var outline in _outlines)
        {
            outline.Item2.IsVisible = OutlineMode != OutlineMode.None;
        }
    }

    private MonoSprite AddMainSprite(SceneNode rootNode, UnitName spriteSheetId, bool isPlayerSide)
    {
        _mainGraphics = CreateMainSprite(spriteSheetId, Vector2.Zero, isPlayerSide, Color.White);

        rootNode.Children.Add(_mainGraphics.Item1);

        return _mainGraphics.Item2;
    }

    private MonoSprite[] AddOutlines(SceneNode rootNode, UnitName spriteSheetId, bool isPlayerSide)
    {
        var sprites = new List<MonoSprite>();

        const int OUTLINE_COUNT = 4;
        const int OUTLINE_LENGTH = 2;
        _outlines = Enumerable.Range(0, OUTLINE_COUNT).Select(x => CreateMainSprite(
            spriteSheetId,
            new Vector2((float)Math.Cos(Math.PI * 2 / OUTLINE_COUNT * x),
                (float)Math.Sin(Math.PI * 2 / OUTLINE_COUNT * x)) * OUTLINE_LENGTH,
            isPlayerSide,
            Color.Red)
        ).ToArray();

        foreach (var outline in _outlines)
        {
            outline.Item2.IsVisible = false;
            sprites.Add(outline.Item2);
            rootNode.Children.Add(outline.Item1);
        }

        return sprites.ToArray();
    }

    private void AddSelectionMarkers(SceneNode rootNode)
    {
        foreach (var selectedMarker in _selectedMarkers)
        {
            rootNode.Children.Add(selectedMarker.Item1);
        }
    }

    private void AddShadowGraphics(SceneNode rootNode, bool isPlayerSide)
    {
        var shadowNode = new SceneNode();

        var teamSpriteEffect = isPlayerSide ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        var shadowSprite = new MonoSprite(_gameObjectContentStorage.GetUnitShadow())
        {
            OriginNormalized = new Vector2(0.5f, 0.5f),
            Color = Color.Lerp(Color.Black, Color.Transparent, 0.5f),
            Effect = teamSpriteEffect
        };
        shadowNode.Entities.Add(new SpriteEntity(shadowSprite));

        rootNode.Children.Add(shadowNode);

        _shadowNode = shadowNode;
    }

    public void RemoveShadowOfCorpse()
    {
        Root.RootNode.Children.Remove(_shadowNode);
    }

    private (SceneNode, MonoSprite) CreateMainSprite(UnitName spriteSheetId, Vector2 startPositionOffset,
        bool isPlayerSide, Color baseColor)
    {
        var sceneNode = new SceneNode
        {
            Position = startPositionOffset
        };
        var texture = _gameObjectContentStorage.GetUnitGraphics(spriteSheetId);
        var teamSpriteEffect = isPlayerSide ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        var origin = isPlayerSide
            ? _graphicsConfig.Origin
            : new Vector2(FRAME_WIDTH - _graphicsConfig.Origin.X, _graphicsConfig.Origin.Y);
        var sprite = new MonoSprite(new TextureRegion2D(texture, new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT)))
        {
            Color = baseColor,
            Origin = origin,
            Effect = teamSpriteEffect
        };
        sceneNode.Entities.Add(new SpriteEntity(sprite));

        return (sceneNode, sprite);
    }

    private static (SceneNode, MonoSprite)[] CreateSelectionMarkers(GameObjectContentStorage gameObjectContentStorage)
    {
        var leftMarkerNode = new SceneNode();
        var leftSprite =
            new MonoSprite(new TextureRegion2D(gameObjectContentStorage.GetCombatantMarkers(),
                new Rectangle(0, 0, 32, 32)))
            {
                OriginNormalized = new Vector2(0.5f, 0.75f)
            };
        leftMarkerNode.Entities.Add(new SpriteEntity(leftSprite));
        var rightMarkerNode = new SceneNode();
        var rightSprite = new MonoSprite(new TextureRegion2D(gameObjectContentStorage.GetCombatantMarkers(),
            new Rectangle(128 - 32, 0, 32, 32)))
        {
            OriginNormalized = new Vector2(0.5f, 0.75f)
        };
        rightMarkerNode.Entities.Add(new SpriteEntity(rightSprite));

        var selectedMarkers = new[]
        {
            (leftMarkerNode, leftSprite),
            (rightMarkerNode, rightSprite)
        };
        return selectedMarkers;
    }

    private void HandleSelectionMarker(GameTime gameTime)
    {
        var isMarkerDisplayed = _currentAnimationFrameSet.IsIdle && ShowActiveMarker;

        foreach (var selectedMarker in _selectedMarkers)
        {
            selectedMarker.Item2.IsVisible = isMarkerDisplayed;
        }

        if (isMarkerDisplayed)
        {
            _selectedMarkerCounter += gameTime.GetElapsedSeconds();
            var t = MathHelper.Clamp((float)Math.Sin(_selectedMarkerCounter * 10), -1f, 0.5f);

            _selectedMarkers[0].Item1.Position = new Vector2(t * 12 - 64, 0);
            _selectedMarkers[1].Item1.Position = new Vector2(128 - t * 12 - 64, 0);
        }
    }

    private void InitializeSprites(UnitName spriteSheetId, Vector2 startPosition, bool isPlayerSide)
    {
        Root = new SceneGraph();
        Root.RootNode.Position = startPosition;

        AddShadowGraphics(Root.RootNode, isPlayerSide);
        AddSelectionMarkers(Root.RootNode);

        var sprites = new List<MonoSprite>();

        var outlineSprites = AddOutlines(Root.RootNode, spriteSheetId, isPlayerSide);
        sprites.AddRange(outlineSprites);

        var mainSprite = AddMainSprite(Root.RootNode, spriteSheetId, isPlayerSide);
        sprites.Add(mainSprite);

        _animatedSprites = sprites.ToArray();
    }

    private void UpdateAnimation(GameTime gameTime)
    {
        _currentAnimationFrameSet.Update(gameTime);

        foreach (var sprite in _animatedSprites)
        {
            sprite.TextureRegion = new TextureRegion2D(_mainTexture, _currentAnimationFrameSet.GetFrameRect());
        }
    }
}