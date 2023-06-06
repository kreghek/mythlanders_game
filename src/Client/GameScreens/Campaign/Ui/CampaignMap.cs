using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
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

    private readonly IList<CampaignButton> _buttonList = new List<CampaignButton>();
    private readonly Texture2D _campaignIconsTexture;
    private readonly IScreen _currentScreen;
    private readonly HeroCampaign _heroCampaign;
    private readonly Texture2D _hudTexture;
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;
    private readonly IScreenManager _screenManager;
    private readonly Texture2D _shadowTexture;

    private TextHint? _currentHint;

    private DragData? _dragData;

    private Rectangle? _graphRect;
    private Vector2 _scroll;

    public CampaignMap(HeroCampaign heroCampaign, IScreenManager screenManager, IScreen currentScreen,
        Texture2D campaignIconsTexture,
        Texture2D backgroundTexture,
        Texture2D shadowTexture,
        Texture2D hudTexture,
        IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _heroCampaign = heroCampaign;
        _screenManager = screenManager;
        _currentScreen = currentScreen;
        _campaignIconsTexture = campaignIconsTexture;
        _backgroundTexture = backgroundTexture;
        _shadowTexture = shadowTexture;
        _hudTexture = hudTexture;
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
        InitChildControls(heroCampaign.Stages, heroCampaign);
    }

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
    /// Scroll to show start node layouts.
    /// </summary>
    public Vector2 StartScroll { get; private set; }

    /// <summary>
    /// State of map.
    /// </summary>
    public MapState State { get; set; }

    /// <summary>
    /// Scroll to show reward node layout.
    /// </summary>
    private Vector2 RewardScroll { get; set; }

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

        foreach (var button in _buttonList)
        {
            var nextNodes = _heroCampaign.Stages.GetNext(button.SourceGraphNodeLayout.Node);

            var nextButtons = _buttonList.Where(x => nextNodes.Contains(x.SourceGraphNodeLayout.Node)).ToArray();

            foreach (var nextButton in nextButtons)
            {
                var buttonPosition = button.Rect.Center.ToVector2();
                var nextPosition = nextButton.Rect.Center.ToVector2();

                var direction = buttonPosition - nextPosition;

                var angle = Math.Atan2(direction.Y, direction.X);

                var startLinePoint = new Vector2((int)(Math.Cos(angle + Math.PI) * 16 + buttonPosition.X),
                    (int)(Math.Sin(angle + Math.PI) * 16 + buttonPosition.Y));
                var targetLinePoint = new Vector2((int)(Math.Cos(angle) * 16 + nextPosition.X),
                    (int)(Math.Sin(angle) * 16 + nextPosition.Y));

                spriteBatch.DrawLine(startLinePoint, targetLinePoint, Color.LightCyan);
            }
        }

        spriteBatch.Draw(_hudTexture, Vector2.Zero, Color.Lerp(Color.White, Color.Transparent, 0.5f));

        if (_currentHint is not null)
        {
            _currentHint.Draw(spriteBatch);
        }
    }

    internal void Update(IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        if (State == MapState.Presentation)
        {
            return;
        }

        foreach (var button in _buttonList)
        {
            button.Update(resolutionIndependentRenderer);
        }

        HandleMapScrollingMyMouse(resolutionIndependentRenderer);
    }

    private void ClearCampaignItemHint()
    {
        _currentHint = null;
    }

    private CampaignButton CreateCampaignButton(HeroCampaign currentCampaign,
        IGraphNodeLayout<ICampaignStageItem> graphNodeLayout)
    {
        var stageItemDisplayName = GetStageItemDisplayName(graphNodeLayout.Node.Payload);
        var stageIconRect = GetStageItemTexture(graphNodeLayout.Node.Payload);
        var stageDisplayInfo = new CampaignStageDisplayInfo(stageItemDisplayName);

        var button = new CampaignButton(new IconData(_campaignIconsTexture, stageIconRect), stageDisplayInfo,
            graphNodeLayout);
        button.OnHover += (_, _) =>
        {
            _currentHint = new TextHint(button.StageInfo.HintText)
            {
                Rect = new Rectangle((button.Rect.Center.ToVector2() + new Vector2(0, 16)).ToPoint(),
                    new Point(200, 50))
            };
        };

        button.OnLeave += (_, _) => { ClearCampaignItemHint(); };

        var trasitionData = new TrasitionData(_currentScreen, _screenManager, currentCampaign);
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
                    SelectCampaignStage(graphNodeLayout.Node, trasitionData);
                }
            }
            else
            {
                if (_heroCampaign.Stages.GetNext(_heroCampaign.CurrentStage).Contains(graphNodeLayout.Node))
                {
                    SelectCampaignStage(graphNodeLayout.Node, trasitionData);
                }
            }
        };
        return button;
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

    private static string GetStageItemDisplayName(ICampaignStageItem campaignStageItem)
    {
        if (campaignStageItem is CombatStageItem)
        {
            return string.Format(UiResource.CampaignStageDisplayNameCombat, 0);
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
        var graphNodeLayouts = horizontalVisualizer.Create(campaignGraph, new VisualizerConfig());

        var random = new Random(currentCampaign.Seed);

        var postProcessors = new ILayoutPostProcessor<ICampaignStageItem>[]
        {
            new PushHorizontallyPostProcessor<ICampaignStageItem>(16),
            new RotatePostProcessor<ICampaignStageItem>(random.NextDouble() * Math.PI),
            new RepeatPostProcessor<ICampaignStageItem>(5,
                new RetryTransformLayoutPostProcessor<ICampaignStageItem>(new RandomPositionLayoutTransformer(random),
                    new IntersectsGraphNodeLayoutValidator<ICampaignStageItem>(), 10))
        };

        foreach (var postProcessor in postProcessors)
        {
            graphNodeLayouts = postProcessor.Process(graphNodeLayouts);
        }

        foreach (var graphNodeLayout in graphNodeLayouts)
        {
            var button = CreateCampaignButton(currentCampaign: currentCampaign, graphNodeLayout: graphNodeLayout);

            _buttonList.Add(button);
        }

        var rewardNodeLayout = graphNodeLayouts.Single(x => x.Node.Payload is RewardStageItem);
        RewardScroll = new Vector2(-rewardNodeLayout.Position.X + _resolutionIndependentRenderer.VirtualBounds.Center.X,
            -rewardNodeLayout.Position.Y + _resolutionIndependentRenderer.VirtualBounds.Center.Y);
        Scroll = RewardScroll;

        var roots = GetRoots(_heroCampaign.Stages);

        var startNodeLayouts = graphNodeLayouts.Where(x => roots.Contains(x.Node));
        var startNodeLayout = startNodeLayouts.First();

        StartScroll = new Vector2(-startNodeLayout.Position.X + _resolutionIndependentRenderer.VirtualBounds.Center.X,
            -startNodeLayout.Position.Y + _resolutionIndependentRenderer.VirtualBounds.Center.Y);

        _graphRect = new Rectangle(
            graphNodeLayouts.Min(x => x.Position.X),
            graphNodeLayouts.Min(x => x.Position.Y),
            graphNodeLayouts.Max(x => x.Position.X + 32),
            graphNodeLayouts.Max(x => x.Position.Y + 32)
        );
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

    private static void SelectCampaignStage(IGraphNode<ICampaignStageItem> stageNode, TrasitionData trasitionData)
    {
        trasitionData.CurrentCampaign.CurrentStage = stageNode;
        stageNode.Payload.ExecuteTransition(trasitionData.CurrentScreen, trasitionData.ScreenManager,
            trasitionData.CurrentCampaign);
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

    private sealed record TrasitionData(IScreen CurrentScreen, IScreenManager ScreenManager,
        HeroCampaign CurrentCampaign);
}