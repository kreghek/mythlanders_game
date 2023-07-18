using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.AnimationFrameSets;
using Client.Core.Campaigns;
using Client.Engine;
using Client.ScreenManagement;

using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Visualization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignMap : ControlBase
{
    public enum MapState
    {
        Presentation,
        Interactive
    }

    private const int LAYOUT_NODE_SIZE = 32;
    private readonly Texture2D _backgroundTexture;

    private readonly IList<CampaignNodeButton> _buttonList = new List<CampaignNodeButton>();
    private readonly Texture2D _campaignIconsTexture;
    private readonly IScreen _currentScreen;
    private readonly HeroCampaign _heroCampaign;
    private readonly Texture2D _hudTexture;
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly IScreenManager _screenManager;
    private readonly Texture2D _shadowTexture;

    private TextHint? _currentHint;

    private DragData? _dragData;

    private Rectangle? _graphRect;

    private float _nodeHighlightCounter;
    private Vector2 _scroll;

    public CampaignMap(HeroCampaign heroCampaign, IScreenManager screenManager, IScreen currentScreen,
        Texture2D campaignIconsTexture,
        Texture2D backgroundTexture,
        Texture2D shadowTexture,
        Texture2D hudTexture,
        IResolutionIndependentRenderer resolutionIndependentRenderer,
        GameObjectContentStorage gameObjectContentStorage)
    {
        _heroCampaign = heroCampaign;
        _screenManager = screenManager;
        _currentScreen = currentScreen;
        _campaignIconsTexture = campaignIconsTexture;
        _backgroundTexture = backgroundTexture;
        _shadowTexture = shadowTexture;
        _hudTexture = hudTexture;
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
        _gameObjectContentStorage = gameObjectContentStorage;
        InitChildControls(heroCampaign.Stages, heroCampaign);
    }

    public PresentationScrollData? Presentation { get; private set; }

    /// <summary>
    /// Current scroll of map.
    /// </summary>
    public Vector2 Scroll
    {
        get => _scroll;
        set => _scroll = _graphRect is not null
            ? NormalizeScroll(value, _graphRect.Value, _resolutionIndependentRenderer.VirtualBounds)
            : value;
    }

    /// <summary>
    /// State of map.
    /// </summary>
    public MapState State { get; set; }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        DrawBackgroundTiles(spriteBatch: spriteBatch, contentRect: contentRect);

        DrawButtonsWithShadows(spriteBatch: spriteBatch, contentRect: contentRect);

        DrawEdges(spriteBatch);

        DrawNodeHighlights(spriteBatch);

        DrawDecorationHud(spriteBatch);

        DrawNodeHint(spriteBatch);
    }

    internal void Update(GameTime gameTime, IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        if (State == MapState.Presentation)
        {
            return;
        }

        foreach (var button in _buttonList)
        {
            button.Update(resolutionIndependentRenderer);
            foreach (var obj in button.DecorativeObjects)
            {
                obj.AnimationFrameSet.Update(gameTime);
            }
        }

        HandleMapScrollingMyMouse(resolutionIndependentRenderer);

        _nodeHighlightCounter += 0.1f;
    }

    private static IReadOnlyCollection<IGraphNodeLayout<ICampaignStageItem>> ApplyPostProcessTransformations(
        HeroCampaign currentCampaign,
        VisualizerConfig visualizatorConfig,
        IReadOnlyCollection<IGraphNodeLayout<ICampaignStageItem>> graphNodeLayouts)
    {
        var random = new Random(currentCampaign.Seed);

        const int REPEAT_ALL_TRANSFORMATIONS_COUNT = 5;
        const int RETRY_TRANSFORMATIONS_COUNT = 10;
        var postProcessors = new ILayoutPostProcessor<ICampaignStageItem>[]
        {
            new PushHorizontallyPostProcessor<ICampaignStageItem>(visualizatorConfig.NodeSize / 2),
            new RotatePostProcessor<ICampaignStageItem>(random.NextDouble() * Math.PI * 0.5f),
            new RepeatPostProcessor<ICampaignStageItem>(REPEAT_ALL_TRANSFORMATIONS_COUNT,
                new RetryTransformLayoutPostProcessor<ICampaignStageItem>(new RandomPositionLayoutTransformer(random),
                    new IntersectsGraphNodeLayoutValidator<ICampaignStageItem>(), RETRY_TRANSFORMATIONS_COUNT))
        };

        foreach (var postProcessor in postProcessors)
        {
            graphNodeLayouts = postProcessor.Process(graphNodeLayouts);
        }

        return graphNodeLayouts;
    }

    private static (Vector2 Start, Vector2 End) CalcLinePoints(Vector2 buttonPosition, Vector2 nextPosition)
    {
        var direction = buttonPosition - nextPosition;

        var angle = Math.Atan2(direction.Y, direction.X);

        var startLinePoint = new Vector2((int)(Math.Cos(angle + Math.PI) * 16 + buttonPosition.X),
            (int)(Math.Sin(angle + Math.PI) * 16 + buttonPosition.Y));
        var targetLinePoint = new Vector2((int)(Math.Cos(angle) * 16 + nextPosition.X),
            (int)(Math.Sin(angle) * 16 + nextPosition.Y));
        return (startLinePoint, targetLinePoint);
    }

    private void ClearCampaignItemHint()
    {
        _currentHint = null;
    }

    private CampaignNodeButton CreateCampaignButton(HeroCampaign currentCampaign,
        IGraphNodeLayout<ICampaignStageItem> graphNodeLayout)
    {
        var stageItemDisplayName = GetStageItemDisplayName(graphNodeLayout.Node.Payload);
        var stageIconRect = GetStageItemTexture(graphNodeLayout.Node.Payload);
        var stageDisplayInfo = new CampaignStageDisplayInfo(stageItemDisplayName);

        var locationNodeState = GetLocationNodeState(graphNodeLayout);

        var button = new CampaignNodeButton(new IconData(_campaignIconsTexture, stageIconRect), stageDisplayInfo,
            graphNodeLayout, locationNodeState);
        button.OnHover += (_, _) =>
        {
            _currentHint = new TextHint(button.StageInfo.HintText)
            {
                Rect = new Rectangle((button.Rect.Center.ToVector2() + new Vector2(0, 16)).ToPoint(),
                    new Point(200, 50))
            };
        };

        button.OnLeave += (_, _) => { ClearCampaignItemHint(); };

        var transitionData = new TransitionData(_currentScreen, _screenManager, currentCampaign);
        button.OnClick += (_, _) =>
        {
            if (State != MapState.Interactive)
            {
                return;
            }

            if (_heroCampaign.CurrentStage is null)
            {
                // Means campaign just started. Available only root nodes.

                var roots = GetRoots(_heroCampaign.Stages);

                if (roots.Contains(button.SourceGraphNodeLayout.Node))
                {
                    SelectCampaignStage(graphNodeLayout.Node, transitionData);
                }
            }
            else
            {
                if (_heroCampaign.Stages.GetNext(_heroCampaign.CurrentStage).Contains(graphNodeLayout.Node))
                {
                    SelectCampaignStage(graphNodeLayout.Node, transitionData);
                }
            }
        };
        return button;
    }

    private PresentationScrollData CreatePresentationScrollData(HeroCampaign currentCampaign,
        IReadOnlyCollection<IGraphNodeLayout<ICampaignStageItem>> graphNodeLayouts)
    {
        if (currentCampaign.CurrentStage is null)
        {
            // First make start position is reward node to show all graph
            // target position - one of start node.

            var rewardNodeLayout = graphNodeLayouts.Single(x => x.Node.Payload is RewardStageItem);

            var roots = GetRoots(_heroCampaign.Stages);

            var startNodeLayouts = graphNodeLayouts.Where(x => roots.Contains(x.Node));
            var startNodeLayout = startNodeLayouts.First();

            return CreatePresentationScrollDataFromNodeLayouts(rewardNodeLayout, startNodeLayout);
        }

        if (currentCampaign.Path.Any())
        {
            // now start node is previous node
            // and target is next available node

            var prevNodeLayout = graphNodeLayouts.Single(x => x.Node == currentCampaign.Path.Last());

            var next = currentCampaign.Stages.GetNext(prevNodeLayout.Node);
            var startNodeLayouts = graphNodeLayouts.Where(x => next.Contains(x.Node));
            var nextNodeLayout = startNodeLayouts.First();

            return CreatePresentationScrollDataFromNodeLayouts(prevNodeLayout, nextNodeLayout);
        }

        var entryLayout = graphNodeLayouts.Single(x => x.Node == currentCampaign.CurrentStage);
        return CreatePresentationScrollDataFromNodeLayouts(entryLayout, entryLayout);
    }

    private PresentationScrollData CreatePresentationScrollDataFromNodeLayouts(
        IGraphNodeLayout<ICampaignStageItem> rewardNodeLayout,
        IGraphNodeLayout<ICampaignStageItem> startNodeLayout)
    {
        var rewardScroll = GetScrollByGraphNodeLayout(rewardNodeLayout);

        var startScroll = GetScrollByGraphNodeLayout(startNodeLayout);

        return new PresentationScrollData(rewardScroll, startScroll);
    }

    private void DrawBackgroundTiles(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        for (var i = -2; i < 10; i++)
        {
            for (var j = -2; j < 10; j++)
            {
                var tilePosition = new Vector2(i * _backgroundTexture.Width, j * _backgroundTexture.Height)
                                   + new Vector2(contentRect.Left + (int)Scroll.X, contentRect.Top + (int)Scroll.Y);
                spriteBatch.Draw(_backgroundTexture,
                    tilePosition, Color.White);
            }
        }
    }

    private void DrawButtonsWithShadows(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        foreach (var button in _buttonList)
        {
            button.Rect = new Rectangle(
                button.SourceGraphNodeLayout.Position.X + contentRect.Left + (int)Scroll.X,
                button.SourceGraphNodeLayout.Position.Y + contentRect.Top + (int)Scroll.Y,
                32, 32);

            spriteBatch.Draw(_shadowTexture,
                new Rectangle(button.Rect.Center.X - _shadowTexture.Width / 2,
                    button.Rect.Center.Y - _shadowTexture.Height / 2, _shadowTexture.Width, _shadowTexture.Height),
                Color.White);
        }

        foreach (var button in _buttonList)
        {
            button.Draw(spriteBatch);
        }
    }

    private void DrawDecorationHud(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_hudTexture, Vector2.Zero, Color.Lerp(Color.White, Color.Transparent, 0.5f));
    }

    private void DrawEdges(SpriteBatch spriteBatch)
    {
        foreach (var button in _buttonList)
        {
            var nextNodes = _heroCampaign.Stages.GetNext(button.SourceGraphNodeLayout.Node);

            var nextButtons = _buttonList.Where(x => nextNodes.Contains(x.SourceGraphNodeLayout.Node)).ToArray();

            foreach (var nextButton in nextButtons)
            {
                var lineColor = GetNodeColorByNodeState(nextButton.NodeState);

                var buttonPosition = button.Rect.Center.ToVector2();
                var nextPosition = nextButton.Rect.Center.ToVector2();

                var (Start, End) = CalcLinePoints(buttonPosition: buttonPosition, nextPosition: nextPosition);

                spriteBatch.DrawLine(Start, End, lineColor);
            }
        }
    }

    private void DrawNodeHighlights(SpriteBatch spriteBatch)
    {
        if (_heroCampaign.CurrentStage is null)
        {
            var roots = GetRoots(_heroCampaign.Stages);
            var rootButtons = _buttonList.Where(x => roots.Contains(x.SourceGraphNodeLayout.Node)).ToArray();

            for (var i = 0; i < rootButtons.Length; i++)
            {
                var button = rootButtons[i];
                spriteBatch.DrawCircle(button.Rect.Center.ToVector2(),
                    24 + (int)(8 * Math.Sin(_nodeHighlightCounter + i * 133)), 16, TestamentColors.MainSciFi);
            }
        }
        else
        {
            var currentButton = _buttonList.Single(x => x.SourceGraphNodeLayout.Node == _heroCampaign.CurrentStage);

            spriteBatch.DrawCircle(currentButton.Rect.Center.ToVector2(), 24, 16, Color.Wheat, 3);

            var next = _heroCampaign.Stages.GetNext(_heroCampaign.CurrentStage);
            var nextButtons = _buttonList.Where(x => next.Contains(x.SourceGraphNodeLayout.Node)).ToArray();

            for (var i = 0; i < nextButtons.Length; i++)
            {
                var button = nextButtons[i];
                spriteBatch.DrawCircle(button.Rect.Center.ToVector2(),
                    16 + (int)(8 * Math.Sin(_nodeHighlightCounter + i * 133)), 16, TestamentColors.MainSciFi, 3);
            }
        }
    }

    private void DrawNodeHint(SpriteBatch spriteBatch)
    {
        _currentHint?.Draw(spriteBatch);
    }

    private CampaignNodeState GetLocationNodeState(IGraphNodeLayout<ICampaignStageItem> graphNodeLayout)
    {
        if (graphNodeLayout.Node == _heroCampaign.CurrentStage)
        {
            return CampaignNodeState.Current;
        }

        if (_heroCampaign.Path.Contains(graphNodeLayout.Node))
        {
            return CampaignNodeState.Passed;
        }

        if (_heroCampaign.CurrentStage is not null)
        {
            var availableNodes = _heroCampaign.Stages.GetAvailableNodes(_heroCampaign.CurrentStage);

            if (!availableNodes.Contains(graphNodeLayout.Node))
            {
                return CampaignNodeState.Unavailable;
            }
        }

        return CampaignNodeState.Available;
    }

    private static Color GetNodeColorByNodeState(CampaignNodeState nodeState)
    {
        return nodeState switch
        {
            CampaignNodeState.Available => TestamentColors.MainSciFi,
            CampaignNodeState.Current or CampaignNodeState.Passed => TestamentColors.MainAncient,
            CampaignNodeState.Unavailable => TestamentColors.Disabled,
            _ => TestamentColors.MainSciFi
        };
    }

    private static IReadOnlyCollection<IGraphNode<ICampaignStageItem>> GetRoots(
        IGraph<ICampaignStageItem> campaignGraph)
    {
        // Look node are not targets for other nodes.

        var nodesOpenList = campaignGraph.GetAllNodes().ToList();

        foreach (var node in nodesOpenList.ToArray())
        {
            var otherNodes = campaignGraph.GetAllNodes().Where(x => x != node).ToArray();

            foreach (var otherNode in otherNodes)
            {
                var nextNodes = campaignGraph.GetNext(otherNode);

                if (nextNodes.Contains(node))
                {
                    nodesOpenList.Remove(node);
                }
            }
        }

        return nodesOpenList;
    }

    private Vector2 GetScrollByGraphNodeLayout(IGraphNodeLayout<ICampaignStageItem> nodeLayout)
    {
        var screenCenter = _resolutionIndependentRenderer.VirtualBounds.Center;
        return new Vector2(
            -nodeLayout.Position.X + screenCenter.X,
            -nodeLayout.Position.Y + screenCenter.Y);
    }

    private static string GetStageItemDisplayName(ICampaignStageItem campaignStageItem)
    {
        if (campaignStageItem is CombatStageItem combat)
        {
            var classSid = combat.CombatSequence.Combats.First().Monsters.First().ClassSid;
            var monsterClass = Enum.Parse<UnitName>(classSid, true);

            var monsteInfo = GameObjectHelper.GetLocalized(monsterClass);
            return UiResource.CampaignStageDisplayNameCombat + "\n" + monsteInfo;
        }

        if (campaignStageItem is RewardStageItem)
        {
            return UiResource.CampaignStageDisplayNameFinish;
        }

        if (campaignStageItem is DialogueEventStageItem)
        {
            return UiResource.CampaignStageDisplayNameTextEvent;
        }

        if (campaignStageItem is RestStageItem)
        {
            return UiResource.CampaignStageDisplayNameRest;
        }

        return UiResource.CampaignStageDisplayNameUnknown;
    }

    private static Rectangle GetStageItemTexture(ICampaignStageItem campaignStageItem)
    {
        var size = new Point(LAYOUT_NODE_SIZE, LAYOUT_NODE_SIZE);

        if (campaignStageItem is CombatStageItem)
        {
            return new Rectangle(new Point(0, 0), size);
        }

        if (campaignStageItem is RewardStageItem)
        {
            return new Rectangle(new Point(1 * LAYOUT_NODE_SIZE, 2 * LAYOUT_NODE_SIZE), size);
        }

        if (campaignStageItem is DialogueEventStageItem)
        {
            return new Rectangle(new Point(1 * LAYOUT_NODE_SIZE, 1 * LAYOUT_NODE_SIZE), size);
        }

        if (campaignStageItem is RestStageItem)
        {
            return new Rectangle(new Point(1 * LAYOUT_NODE_SIZE, 1 * LAYOUT_NODE_SIZE), size);
        }

        return new Rectangle(new Point(2 * LAYOUT_NODE_SIZE, 2 * LAYOUT_NODE_SIZE), size);
    }

    private void HandleMapScrollingMyMouse(IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        var mouseState = Mouse.GetState();
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            var mousePosition = mouseState.Position.ToVector2();

            var rirMousePosition =
                resolutionIndependentRenderer.ConvertScreenToWorldCoordinates(mousePosition);

            if (_dragData is null)
            {
                _dragData = new DragData(Scroll, rirMousePosition);
                ClearCampaignItemHint();
            }

            Scroll = _dragData.DragScroll - (_dragData.StartMousePosition - rirMousePosition);
        }
        else
        {
            _dragData = null;
        }
    }

    private void InitChildControls(IGraph<ICampaignStageItem> campaignGraph, HeroCampaign currentCampaign)
    {
        var horizontalVisualizer = new HorizontalGraphVisualizer<ICampaignStageItem>();
        var visualizerConfig = new VisualizerConfig();
        var graphNodeLayouts = horizontalVisualizer.Create(campaignGraph, visualizerConfig);
        graphNodeLayouts = ApplyPostProcessTransformations(currentCampaign, visualizerConfig, graphNodeLayouts);

        foreach (var graphNodeLayout in graphNodeLayouts)
        {
            var button = CreateCampaignButton(currentCampaign, graphNodeLayout);

            if (button.SourceGraphNodeLayout.Node.Payload is CombatStageItem combatStageItem)
            {
                var monster = combatStageItem.CombatSequence.Combats.First().Monsters.First();
                var monsterTexture =
                    _gameObjectContentStorage.GetUnitGraphics(Enum.Parse<UnitName>(monster.ClassSid, true));

                var grayscaleTexture = CreateAnimationSequenceTexture(monsterTexture, new Rectangle(0, 0, 128, 128));

                button.DecorativeObjects.Add(new CampaignMapDecorativeObject(grayscaleTexture,
                    new LinearAnimationFrameSet(new[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 4, 128, 128, 8) { IsLooping = true },
                    new Vector2(16, 0)));
            }

            _buttonList.Add(button);
        }

        Presentation = CreatePresentationScrollData(currentCampaign, graphNodeLayouts);
        Scroll = Presentation.Start;

        _graphRect = new Rectangle(
            graphNodeLayouts.Min(x => x.Position.X),
            graphNodeLayouts.Min(x => x.Position.Y),
            graphNodeLayouts.Max(x => x.Position.X + 32),
            graphNodeLayouts.Max(x => x.Position.Y + 32)
        );
    }

    private Texture2D CreateAnimationSequenceTexture(Texture2D sourceTexture, Rectangle sourceRect)
    {
        var grayScaleTexture = CreateGrayscaleTexture(sourceTexture, sourceRect);
        
        var graphicDevice = _resolutionIndependentRenderer.ViewportAdapter.GraphicsDevice;

        //initialize a texture
        var width = sourceRect.Width;
        var height = sourceRect.Height;
        const int FRAME_COUNT = 8;
        Texture2D texture = new Texture2D(graphicDevice, width * FRAME_COUNT, height);
        
        var count = width * height;
        Color[] sourceData = new Color[count];
        grayScaleTexture.GetData(0, sourceRect, sourceData, 0, count);

        for (var frameIndex = 0; frameIndex < FRAME_COUNT; frameIndex++)
        {
            Color[] data = new Color[count];

            if (frameIndex % 7 == 0)
            {
                for (int pixel = 0; pixel < data.Length; pixel++)
                {
                    data[pixel] = new Color(0, 0, 0, 0);
                }
            }
            else
            {
                for (int pixel = 0; pixel < data.Length; pixel++)
                {
                    var oc = sourceData[pixel];
                    int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    
                    if (pixel * frameIndex * 133 % 13 == 0)
                    {
                        data[pixel] = new Color(0, 0, 0, 0);
                    }
                    else
                    {
                        // Original pixel
                        
                        data[pixel] = new Color(grayScale, grayScale, grayScale, oc.A);
                    }
                }
            }

            //set the color
            texture.SetData(0, new Rectangle(frameIndex * width, 0, width, height), data, 0, count);
        }

        return texture;
    }

    private Texture2D CreateGrayscaleTexture(Texture2D sourceTexture, Rectangle sourceRect)
    {
        var graphicDevice = _resolutionIndependentRenderer.ViewportAdapter.GraphicsDevice;

        //initialize a texture
        var width = sourceRect.Width;
        var height = sourceRect.Height;
        Texture2D texture = new Texture2D(graphicDevice, width, height);

        var count = width * height;
        Color[] sourceData = new Color[count];
        sourceTexture.GetData(0, sourceRect, sourceData, 0, count);

        //the array holds the color for each pixel in the texture
        Color[] data = new Color[count];
        for (int pixel = 0; pixel < data.Length; pixel++)
        {
            //the function applies the color according to the specified pixel
            var oc = sourceData[pixel];
            int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
            data[pixel] = new Color(grayScale, grayScale, grayScale, oc.A);
        }

        //set the color
        texture.SetData(data);

        return texture;
    }

    private static Vector2 NormalizeScroll(Vector2 currentScroll, Rectangle boundingGraphRect,
        Rectangle virtualBounding)
    {
        var scroll = currentScroll;

        if (currentScroll.X > -(boundingGraphRect.Left - virtualBounding.Center.X))
        {
            scroll.X = -(boundingGraphRect.Left - virtualBounding.Center.X);
        }

        if (currentScroll.X < -(boundingGraphRect.Right - virtualBounding.Center.X))
        {
            scroll.X = -(boundingGraphRect.Right - virtualBounding.Center.X);
        }

        if (currentScroll.Y > -(boundingGraphRect.Top - virtualBounding.Center.Y))
        {
            scroll.Y = -(boundingGraphRect.Top - virtualBounding.Center.Y);
        }

        if (currentScroll.Y < -(boundingGraphRect.Bottom - virtualBounding.Center.Y))
        {
            scroll.Y = -(boundingGraphRect.Bottom - virtualBounding.Center.Y);
        }

        return scroll;
    }

    private static void SelectCampaignStage(IGraphNode<ICampaignStageItem> stageNode, TransitionData transitionData)
    {
        var campaign = transitionData.CurrentCampaign;
        if (campaign.CurrentStage is not null)
        {
            campaign.Path.Add(campaign.CurrentStage);
        }

        campaign.CurrentStage = stageNode;
        stageNode.Payload.ExecuteTransition(transitionData.CurrentScreen, transitionData.ScreenManager,
            campaign);
    }

    private sealed record DragData(Vector2 DragScroll, Vector2 StartMousePosition);

    private sealed class VisualizerConfig : ILayoutConfig
    {
        /// <inheritdoc />
        public int NodeSize => LAYOUT_NODE_SIZE * 2 + CONTENT_MARGIN * 2;
    }

    private sealed class RandomPositionLayoutTransformer : IGraphNodeLayoutTransformer<ICampaignStageItem>
    {
        private readonly Random _random;

        public RandomPositionLayoutTransformer(Random random)
        {
            _random = random;
        }

        /// <inheritdoc />
        public IGraphNodeLayout<ICampaignStageItem> Get(IGraphNodeLayout<ICampaignStageItem> layout)
        {
            var offset = new Position(_random.Next(-20, 20), _random.Next(-20, 20));
            var position = new Position(layout.Position.X + offset.X, layout.Position.Y + offset.Y);

            return new GraphNodeLayout<ICampaignStageItem>(layout.Node, position, layout.Size);
        }
    }

    private sealed record TransitionData(IScreen CurrentScreen, IScreenManager ScreenManager,
        HeroCampaign CurrentCampaign);

    public sealed record PresentationScrollData(Vector2 Start, Vector2 Target);
}

public static class GraphExtensions
{
    public static IEnumerable<IGraphNode<T>> GetAvailableNodes<T>(this IGraph<T> graph, IGraphNode<T> startNode)
    {
        return Enumerable.Repeat(startNode, 1)
            .Concat(graph.GetNext(startNode).SelectMany(graph.GetAvailableNodes))
            .Distinct();
    }

    public static bool HasWayBetween<T>(this IGraph<T> graph, IGraphNode<T> nodeFrom,
        IGraphNode<T> nodeTo)
    {
        if (nodeFrom == nodeTo)
        {
            return true;
        }

        return graph.GetNext(nodeFrom)
            .Select(next => graph.HasWayBetween(next, nodeTo))
            .Any(x => x);
    }
}