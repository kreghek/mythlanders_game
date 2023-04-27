using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
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

internal sealed class CampaignPanel : ControlBase
{
    private readonly Texture2D _campaignIconsTexture;
    private readonly IScreen _currentScreen;
    private readonly HeroCampaign _heroCampaign;
    private readonly IScreenManager _screenManager;
    private TextHint? _currentHint;

    public CampaignPanel(HeroCampaign heroCampaign, IScreenManager screenManager, IScreen currentScreen,
        Texture2D campaignIconsTexture,
        ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _heroCampaign = heroCampaign;
        _screenManager = screenManager;
        _currentScreen = currentScreen;
        _campaignIconsTexture = campaignIconsTexture;
        ResolutionIndependentRenderer = resolutionIndependentRenderer;
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

    public Vector2 Scroll { get; set; }
    public ResolutionIndependentRenderer ResolutionIndependentRenderer { get; }

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
        foreach (var button in _buttonList)
        {
            button.Update(resolutionIndependentRenderer);
        }

        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position.ToVector2();

            var rirPosition =
                resolutionIndependentRenderer.ScaleMouseToScreenCoordinates(mousePosition);

            if (_mouseDrag is null)
            {
                _mouseDrag = rirPosition;
                _oldScroll = Scroll;
            }

            Scroll = _oldScroll.Value - (_mouseDrag.Value - rirPosition);
        }
        else
        {
            _mouseDrag = null;
            _oldScroll = null;
        }
    }

    private Vector2? _oldScroll;

    private Vector2? _mouseDrag;
    
    private sealed class VisualizerConfig : ILayoutConfig
    {
        public int NodeSize => 32 * 2 + CONTENT_MARGIN * 2;
    }
    
    private static Rectangle GetStageItemTexture(ICampaignStageItem campaignStageItem)
    {
        var size = new Point(32, 32);

        if (campaignStageItem is CombatStageItem)
        {
            return new Rectangle(new Point(0, 0), size);
        }

        if (campaignStageItem is RewardStageItem)
        {
            return new Rectangle(new Point(1 * 32, 2 * 32), size);
        }

        if (campaignStageItem is DialogueEventStageItem)
        {
            return new Rectangle(new Point(1 * 32, 1 * 32), size);
        }

        if (campaignStageItem is RestStageItem)
        {
            return new Rectangle(new Point(1 * 32, 1 * 32), size);
        }

        return new Rectangle(new Point(2 * 32, 2 * 32), size);
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

    private sealed class Transformer : IGraphNodeLayoutTransformer<ICampaignStageItem>
    {
        private readonly Random _random;

        public Transformer(Random random)
        {
            _random = random;
        }

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
            new RepeatPostProcessor<ICampaignStageItem>(5, new RetryTransformLayoutPostProcessor<ICampaignStageItem>(new Transformer(random), new IntersectsGraphNodeLayoutValidator<ICampaignStageItem>(), 10))
        };

        foreach (var postProcessor in postProcessors)
        {
            graphNodeLayouts = postProcessor.Process(graphNodeLayouts);
        }

        foreach (var graphNodeLayout in graphNodeLayouts)
        {
            var stageItemDisplayName = GetStageItemDisplayName(graphNodeLayout.Node.Payload);
            var stageIconRect = GetStageItemTexture(graphNodeLayout.Node.Payload);
            
            var button = new CampaignButton(new IconData(_campaignIconsTexture, stageIconRect), stageItemDisplayName, graphNodeLayout.Position, graphNodeLayout);
            button.OnHover += (s, e) =>
            {
                _currentHint = new TextHint(button.Description)
                {
                    Rect = new Rectangle((button.Rect.Center.ToVector2() + new Vector2(0, 16)).ToPoint(), new Point(200, 50))
                };
            };
            button.OnClick += (s, e) =>
            {
                graphNodeLayout.Node.Payload.ExecuteTransition(_currentScreen, _screenManager, currentCampaign);
            };

            _buttonList.Add(button);
        }

        var rewardNodeLayout = graphNodeLayouts.Single(x => x.Node.Payload is RewardStageItem);
        RewardScroll = new Vector2(-rewardNodeLayout.Position.X + ResolutionIndependentRenderer.VirtualBounds.Center.X, -rewardNodeLayout.Position.Y + ResolutionIndependentRenderer.VirtualBounds.Center.Y);
        Scroll = RewardScroll;

        var roots = GetRoots(_heroCampaign.Stages);

        var startNodeLayouts = graphNodeLayouts.Where(x => roots.Contains(x.Node));
        var startNodeLayout = startNodeLayouts.First();

        StartScroll = new Vector2(-startNodeLayout.Position.X + ResolutionIndependentRenderer.VirtualBounds.Center.X, -startNodeLayout.Position.Y + ResolutionIndependentRenderer.VirtualBounds.Center.Y);

        _graphRect = new Rectangle(
            graphNodeLayouts.Min(x => x.Position.X),
            graphNodeLayouts.Min(x => x.Position.Y),
            graphNodeLayouts.Max(x => x.Position.X + 32),
            graphNodeLayouts.Max(x => x.Position.Y + 32)
        );
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

    public Vector2 RewardScroll { get; private set; }
    public Vector2 StartScroll { get; private set; }
}