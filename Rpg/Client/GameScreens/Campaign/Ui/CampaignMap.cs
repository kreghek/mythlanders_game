using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core.Campaigns;

using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Visualization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignMap : ControlBase
{
    private readonly Texture2D _campaignIconsTexture;
    private readonly IScreen _currentScreen;
    private readonly HeroCampaign _heroCampaign;
    private readonly IScreenManager _screenManager;
    private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
    
    private TextHint? _currentHint;

    public CampaignMap(HeroCampaign heroCampaign, IScreenManager screenManager, IScreen currentScreen,
        Texture2D campaignIconsTexture,
        ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _heroCampaign = heroCampaign;
        _screenManager = screenManager;
        _currentScreen = currentScreen;
        _campaignIconsTexture = campaignIconsTexture;
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
        InitChildControls(heroCampaign.Stages, heroCampaign);
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    /// <summary>
    /// Current scroll of map.
    /// </summary>
    public Vector2 Scroll
    {
        get => _scroll;
        set
        {
            _scroll = _graphRect is not null ? NormalizeScroll(value, _graphRect.Value) : value;
        } 
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        foreach (var button in _buttonList)
        {
            button.Rect = new Rectangle(
                button.Position.X + contentRect.Left + (int)Scroll.X,
                button.Position.Y + contentRect.Top+ (int)Scroll.Y,
                32, 32);
            button.Draw(spriteBatch);
        }

        foreach (var button in _buttonList)
        {
            var nextNodes = _heroCampaign.Stages.GetNext(button.GraphNodeLayout.Node);

            var nextButtons = _buttonList.Where(x => nextNodes.Contains(x.GraphNodeLayout.Node)).ToArray();

            foreach (var nextButton in nextButtons)
            {
                var buttonPosition = button.Rect.Center.ToVector2();
                var nextPosition = nextButton.Rect.Center.ToVector2();

                var direction = buttonPosition - nextPosition;

                var angle = Math.Atan2(direction.Y, direction.X);

                var startLinePoint = new Vector2((int)(Math.Cos(angle + Math.PI) * 16 + buttonPosition.X), (int)(Math.Sin(angle + Math.PI) * 16 + buttonPosition.Y));
                var targetLinePoint = new Vector2((int)(Math.Cos(angle ) * 16 + nextPosition.X), (int)(Math.Sin(angle) * 16 + nextPosition.Y));

                spriteBatch.DrawLine(startLinePoint, targetLinePoint, Color.LightCyan);
            }
        }

        if (_currentHint is not null)
        {
            _currentHint.Draw(spriteBatch);
        }
    }

    internal void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
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

    private void HandleMapScrollingMyMouse(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        var mouseState = Mouse.GetState();
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            var mousePosition = mouseState.Position.ToVector2();

            var rirMousePosition =
                resolutionIndependentRenderer.ScaleMouseToScreenCoordinates(mousePosition);

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

    private DragData? _dragData;

    private sealed record DragData(Vector2 DragScroll, Vector2 StartMousePosition);

    private void ClearCampaignItemHint()
    {
        _currentHint = null;
    }

    private static Vector2 NormalizeScroll(Vector2 currentScroll, Rectangle boundingGraphRect)
    {
        var scroll = currentScroll;
        
        if (currentScroll.X < boundingGraphRect.Left)
        {
            scroll.X = boundingGraphRect.Left;
        }

        if (currentScroll.X > boundingGraphRect.Right)
        {
            scroll.X = boundingGraphRect.Right;
        }
        
        if (currentScroll.Y < boundingGraphRect.Top)
        {
            scroll.Y = boundingGraphRect.Top;
        }

        if (currentScroll.Y > boundingGraphRect.Bottom)
        {
            scroll.Y = boundingGraphRect.Bottom;
        }

        return scroll;
    }
    
    public enum MapState
    {
        Presentation,
        Interactive
    }

    /// <summary>
    /// State of map.
    /// </summary>
    public MapState State { get; set; }

    private sealed class VisualizerConfig : ILayoutConfig
    {
        /// <inheritdoc />
        public int NodeSize => LAYOUT_NODE_SIZE * 2 + CONTENT_MARGIN * 2;
    }

    private const int LAYOUT_NODE_SIZE = 32;
    
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

    private readonly IList<CampaignButton> _buttonList = new List<CampaignButton>();

    private Rectangle? _graphRect;
    private Vector2 _scroll;

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

    private void InitChildControls(IGraph<ICampaignStageItem> campaignGraph, HeroCampaign currentCampaign)
    {
        var horizontalVisualizer = new HorizontalGraphVisualizer<ICampaignStageItem>();
        var graphNodeLayouts = horizontalVisualizer.Create(campaignGraph, new VisualizerConfig());

        var random = new Random(1);

        var postProcessors = new ILayoutPostProcessor<ICampaignStageItem>[] { 
            new PushHorizontallyPostProcessor<ICampaignStageItem>(16),
            new RotatePostProcessor<ICampaignStageItem>(random.NextDouble() * Math.PI),
            new RepeatPostProcessor<ICampaignStageItem>(5, new RetryTransformLayoutPostProcessor<ICampaignStageItem>(new RandomPositionLayoutTransformer(random), new IntersectsGraphNodeLayoutValidator<ICampaignStageItem>(), 10))
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
        RewardScroll = new Vector2(-rewardNodeLayout.Position.X + _resolutionIndependentRenderer.VirtualBounds.Center.X, -rewardNodeLayout.Position.Y + _resolutionIndependentRenderer.VirtualBounds.Center.Y);
        Scroll = RewardScroll;

        var roots = GetRoots(_heroCampaign.Stages);

        var startNodeLayouts = graphNodeLayouts.Where(x => roots.Contains(x.Node));
        var startNodeLayout = startNodeLayouts.First();

        StartScroll = new Vector2(-startNodeLayout.Position.X + _resolutionIndependentRenderer.VirtualBounds.Center.X, -startNodeLayout.Position.Y + _resolutionIndependentRenderer.VirtualBounds.Center.Y);

        _graphRect = new Rectangle(
            graphNodeLayouts.Min(x => x.Position.X),
            graphNodeLayouts.Min(x => x.Position.Y),
            graphNodeLayouts.Max(x => x.Position.X + 32),
            graphNodeLayouts.Max(x => x.Position.Y + 32)
        );
    }

    private CampaignButton CreateCampaignButton(HeroCampaign currentCampaign, IGraphNodeLayout<ICampaignStageItem> graphNodeLayout)
    {
        var stageItemDisplayName = GetStageItemDisplayName(graphNodeLayout.Node.Payload);
        var stageIconRect = GetStageItemTexture(graphNodeLayout.Node.Payload);

        var button = new CampaignButton(new IconData(_campaignIconsTexture, stageIconRect), stageItemDisplayName,
            graphNodeLayout.Position, graphNodeLayout);
        button.OnHover += (_, _) =>
        {
            _currentHint = new TextHint(button.Description)
            {
                Rect = new Rectangle((button.Rect.Center.ToVector2() + new Vector2(0, 16)).ToPoint(), new Point(200, 50))
            };
        };

        button.OnLeave += (_, _) => { ClearCampaignItemHint(); };

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

                if (roots.Contains(button.GraphNodeLayout.Node))
                {
                    graphNodeLayout.Node.Payload.ExecuteTransition(_currentScreen, _screenManager,
                        currentCampaign);
                }
            }
            else
            {
                if (_heroCampaign.Stages.GetNext(_heroCampaign.CurrentStage).Contains(graphNodeLayout.Node))
                {
                    graphNodeLayout.Node.Payload.ExecuteTransition(_currentScreen, _screenManager,
                        currentCampaign);
                }
            }
        };
        return button;
    }

    private static IReadOnlyCollection<IGraphNode<ICampaignStageItem>> GetRoots(IGraph<ICampaignStageItem> campaignGraph)
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

    /// <summary>
    /// Scroll to show reward node layout.
    /// </summary>
    private Vector2 RewardScroll { get; set; }
    
    /// <summary>
    /// Scroll to show start node layouts.
    /// </summary>
    public Vector2 StartScroll { get; private set; }
}