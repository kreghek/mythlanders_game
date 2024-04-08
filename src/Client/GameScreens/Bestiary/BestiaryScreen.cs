using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Engine;
using Client.GameScreens.Bestiary.Ui;
using Client.GameScreens.Campaign;
using Client.GameScreens.CommandCenter;
using Client.ScreenManagement;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Bestiary;

internal sealed class BestiaryScreen : GameScreenWithMenuBase
{
    private readonly ScreenTransition _parentScreen;
    private readonly object _parentScreenArgs;
    private readonly IList<ButtonBase> _monstersButtonList;
    private readonly ButtonBase _knowledgeTabButton;
    private readonly Player _player;
    private readonly IUiContentStorage _uiContentStorage;
    private readonly ICharacterCatalog _unitSchemeCatalog;
    private readonly ButtonBase _monsterPerkTabButton;

    private MonsterPerksPanel _perksPanel = null!;

    private UnitScheme? _selectedMonster;

    public BestiaryScreen(MythlandersGame game, BestiaryScreenTransitionArguments args)
        : base(game)
    {
        _parentScreenArgs = args.ParentScreenArgs;
        _parentScreen = args.ParentScreen;

        _uiContentStorage = game.Services.GetService<IUiContentStorage>();

        _unitSchemeCatalog = game.Services.GetService<ICharacterCatalog>();
        var globeProvider = game.Services.GetService<GlobeProvider>();
        _player = globeProvider.Globe.Player;

        _monstersButtonList = new List<ButtonBase>();

        _knowledgeTabButton = new ResourceTextButton(nameof(UiResource.BestiaryTabMonsterKnowledgeCaption));
        _knowledgeTabButton.OnClick += (s, e) => { _currentTab = BestiaryTab.Knowledge; };

        _monsterPerkTabButton = new ResourceTextButton(nameof(UiResource.BestiaryTabMonsterPerksCaption));
        _monsterPerkTabButton.OnClick += (s, e) => { _currentTab = BestiaryTab.Perks; };
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle));

        backButton.OnClick += (_, _) =>
        {
            switch (_parentScreen)
            {
                case ScreenTransition.CommandCenter:
                    ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter,
                        (CommandCenterScreenTransitionArguments)_parentScreenArgs);
                    break;

                case ScreenTransition.Campaign:
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                        (CampaignScreenTransitionArguments)_parentScreenArgs);
                    break;
            }

        };

        return new ButtonBase[] { backButton };
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        ResolutionIndependentRenderer.BeginDraw();
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        const int BUTTON_WIDTH = 100;

        var tabPosition = contentRect.Left + ControlBase.CONTENT_MARGIN;

        if (_player.KnownMonsters.Any())
        {
            _knowledgeTabButton.Rect = new Rectangle(tabPosition, contentRect.Top + ControlBase.CONTENT_MARGIN, BUTTON_WIDTH, 20);
            _knowledgeTabButton.Draw(spriteBatch);

            tabPosition += _knowledgeTabButton.Rect.Width + ControlBase.CONTENT_MARGIN;
        }

        if (_player.MonsterPerks.Any())
        {
            _monsterPerkTabButton.Rect = new Rectangle(tabPosition, contentRect.Top + ControlBase.CONTENT_MARGIN, BUTTON_WIDTH, 20);
            _monsterPerkTabButton.Draw(spriteBatch);
        }

        var tabBodyRect = new Rectangle(
            tabPosition,
            contentRect.Top + ControlBase.CONTENT_MARGIN + ControlBase.CONTENT_MARGIN + 20, 
            contentRect.Width - ControlBase.CONTENT_MARGIN * 2,
            contentRect.Height - ControlBase.CONTENT_MARGIN * 2);

        switch (_currentTab)
        {
            case BestiaryTab.Knowledge:
                DrawKnowledge(spriteBatch, tabBodyRect);
                break;
            case BestiaryTab.Perks:
                DrawPerks(spriteBatch, tabBodyRect);
                break;
            default:
                break;
        }

        spriteBatch.End();
    }

    private void DrawPerks(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var descriptionText = StringHelper.LineBreaking(UiResource.BestiaryTabMonsterPerksDescription, 60);
        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), descriptionText, contentRect.Location.ToVector2() + new Vector2(ControlBase.CONTENT_MARGIN), MythlandersColors.Description);

        var descriptionSize = _uiContentStorage.GetTitlesFont().MeasureString(descriptionText);

        _perksPanel.Rect =
            new Rectangle(
                contentRect.Left + ControlBase.CONTENT_MARGIN, 
                contentRect.Top + ControlBase.CONTENT_MARGIN + (int)descriptionSize.Y + ControlBase.CONTENT_MARGIN, 
                contentRect.Width - ControlBase.CONTENT_MARGIN * 2,
                contentRect.Height - ControlBase.CONTENT_MARGIN * 2);

        _perksPanel.Draw(spriteBatch);
    }

    private void DrawKnowledge(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var descriptionText = StringHelper.LineBreaking(UiResource.BestiaryTabMonsterKnowledgeDescription, 60);
        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), descriptionText, contentRect.Location.ToVector2() + new Vector2(ControlBase.CONTENT_MARGIN), MythlandersColors.Description);

        var descriptionSize = _uiContentStorage.GetTitlesFont().MeasureString(descriptionText);

        for (var index = 0; index < _monstersButtonList.Count; index++)
        {
            var button = _monstersButtonList[index];
            button.Rect = new Rectangle(contentRect.Left + ControlBase.CONTENT_MARGIN, contentRect.Top + (int)descriptionSize.Y + ControlBase.CONTENT_MARGIN + index * (20 + ControlBase.CONTENT_MARGIN), 100, 20);
            button.Draw(spriteBatch);
        }

        if (_selectedMonster is not null)
        {
            var sb = CollectMonsterStats(_selectedMonster);

            for (var statIndex = 0; statIndex < sb.Count; statIndex++)
            {
                var line = sb[statIndex];
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), line,
                    new Vector2(contentRect.Left + ControlBase.CONTENT_MARGIN + 100 + ControlBase.CONTENT_MARGIN, contentRect.Top + (int)descriptionSize.Y + ControlBase.CONTENT_MARGIN + statIndex * 22), Color.White);
            }
        }
    }

    protected override void InitializeContent()
    {
        InitializeMonsterButtons();

        _perksPanel = new MonsterPerksPanel(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
            Game.Content.Load<Texture2D>("Sprites/GameObjects/MonsterPerkIcons"),
            UiThemeManager.UiContentStorage.GetTitlesFont(), UiThemeManager.UiContentStorage.GetMainFont(),
            _player.MonsterPerks);
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        UpdateTabs();

        switch (_currentTab)
        {
            case BestiaryTab.Knowledge:
                foreach (var button in _monstersButtonList)
                {
                    button.Update(ResolutionIndependentRenderer);
                }
                break;
            case BestiaryTab.Perks:
                break;
            default:
                break;
        }
    }

    private void UpdateTabs()
    {
        if (_player.KnownMonsters.Any())
        {
            _knowledgeTabButton.Update(ResolutionIndependentRenderer);
        }

        if (_player.MonsterPerks.Any())
        {
            _monsterPerkTabButton.Update(ResolutionIndependentRenderer);
        }
    }

    private static IList<string> CollectMonsterStats(UnitScheme monsterScheme)
    {
        var unitName = monsterScheme.Name;
        var name = GameObjectHelper.GetLocalized(unitName);

        var sb = new List<string>()
        {
            name,
             string.Format(UiResource.HitPointsLabelTemplate, "???"),
             string.Format(UiResource.ShieldPointsLabelTemplate, "???")
         };
        //
        // foreach (var perk in monster.Perks)
        // {
        //     var localizedName = GameObjectHelper.GetLocalized(perk);
        //     sb.Add(localizedName);
        //
        //     var localizedDescription = GameObjectHelper.GetLocalizedDescription(perk);
        //
        //     sb.Add(localizedDescription);
        // }
        //
        // foreach (var skill in monster.Skills)
        // {
        //     var localizedName = GameObjectHelper.GetLocalized(skill.Sid);
        //
        //     sb.Add(localizedName);
        // }

        return sb;
    }

    private BestiaryTab _currentTab;

    private void InitializeMonsterButtons()
    {
        var monsterSchemes = _unitSchemeCatalog.AllMonsters;

        _monstersButtonList.Clear();

        foreach (var monsterScheme in monsterSchemes)
        {
            if (!_player.KnownMonsters.Any(x => string.Equals(x.ClassSid, monsterScheme.Name.ToString(), System.StringComparison.CurrentCultureIgnoreCase)))
            {
                // This monster is unknown. So do not display it on the screen.
                continue;
            }

            var unitName = monsterScheme.Name;
            var name = GameObjectHelper.GetLocalized(unitName);

            var button = new TextButton(name);
            button.OnClick += (_, _) => { _selectedMonster = monsterScheme; };
            _monstersButtonList.Add(button);
        }
    }

    private enum BestiaryTab
    {
        Knowledge,
        Perks
    }
}