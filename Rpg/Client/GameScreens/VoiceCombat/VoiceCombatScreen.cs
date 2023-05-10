using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;

using Core.Dices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.VoiceCombat;

internal class VoiceCombatScreen : CombatScreenBase
{
    private class VoiceCombatant
    {
        public Dictionary<VoiceCombatantStatType, int> Stats { get; }
        public UnitName Sid { get; }

        public VoiceCombatant(int conviction, int aspiration, UnitName sid)
        {
            Stats = new Dictionary<VoiceCombatantStatType, int>
            {
                { VoiceCombatantStatType.Conviction, conviction },
                { VoiceCombatantStatType.Aspiration, aspiration }
            };

            Sid = sid;
        }
    }

    private readonly HeroCampaign _campaign;

    private readonly VoiceCombatant _leftCombatant;
    private readonly VoiceCombatant _rightCombatant;

    private readonly VoiceCombatOptions _voiceCombatOptions;
    private VoiceCombatMove? _intentedCpuMove;

    public VoiceCombatScreen(TestamentGame game, VoiceCombatScreenTransitionArguments args) : base(game, args.Campaign)
    {
        _campaign = args.Campaign;

        _uiContentStorage = game.Services.GetService<IUiContentStorage>();
        _dice = game.Services.GetService<IDice>();

        _leftCombatant = new VoiceCombatant(13, 10, UnitName.Assaulter);
        _rightCombatant = new VoiceCombatant(10, 8, UnitName.ChineseOldman);

        _voiceCombatOptions = new VoiceCombatOptions();

        _availableHeroMoves = new List<VoiceCombatMove> { 
            new("HaventBeenPunchedInTheFaceForALongTime", VoiceCombatMoveType.Domination, new VoiceCombatMoveDamage(VoiceCombatantStatType.Conviction, 1)),
            new("TheGoodWeather", VoiceCombatMoveType.Diplomacy, new VoiceCombatMoveDamage(VoiceCombatantStatType.Conviction, 1)),
            new("TwoPlusTwoIsFive", VoiceCombatMoveType.Sophistic, new VoiceCombatMoveDamage(VoiceCombatantStatType.Conviction, 1))
        };

        _availableCpuMoves = new List<VoiceCombatMove> {
            new("HaventBeenPunchedInTheFaceForALongTime", VoiceCombatMoveType.Domination, new VoiceCombatMoveDamage(VoiceCombatantStatType.Conviction, 1)),
            new("TheGoodWeather", VoiceCombatMoveType.Diplomacy, new VoiceCombatMoveDamage(VoiceCombatantStatType.Conviction, 1)),
            new("TwoPlusTwoIsFive", VoiceCombatMoveType.Sophistic, new VoiceCombatMoveDamage(VoiceCombatantStatType.Conviction, 1))
        };

        _currentNpcMoves = new List<VoiceCombatMove>();

        SelectNewOptionSet();

        IntentCpuMove();
    }

    private void IntentCpuMove()
    {
        _intentedCpuMove = _dice.RollFromList(_availableCpuMoves.ToArray());
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var closeButton = new ResourceTextButton(nameof(UiResource.SkipButtonTitle));
        closeButton.OnClick += CloseButton_OnClick;

        return new ButtonBase[]
        {
            closeButton
        };
    }


    protected override void InitializeContent()
    {
        InitDialogueControls();
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        _voiceCombatOptions.Update(ResolutionIndependentRenderer);
    }

    private sealed class VoiceCombatMove {
        public VoiceCombatMove(string textSid, VoiceCombatMoveType type, VoiceCombatMoveDamage damage)
        {
            TextSid = textSid;
            Type = type;
            Damage = damage;
        }

        public string TextSid { get; }

        public VoiceCombatMoveType Type { get; }
        public VoiceCombatMoveDamage Damage { get; }
    }

    private enum VoiceCombatMoveType
    {
        Diplomacy,
        Domination,
        Sophistic
    }

    private enum VoiceCombatantStatType
    {
        Conviction,
        Aspiration
    }

    private sealed record VoiceCombatMoveDamage(VoiceCombatantStatType TargetStat, int Value);

    private const int HERO_OPTION_COUNT = 3;

    private readonly IList<VoiceCombatMove> _availableHeroMoves;
    private readonly IList<VoiceCombatMove> _availableCpuMoves;
    private readonly IList<VoiceCombatMove> _currentNpcMoves;
    private readonly IUiContentStorage _uiContentStorage;
    private readonly IDice _dice;

    private void InitDialogueControls()
    {
        CreateOptionControls();
    }

    private void CreateOptionControls()
    {
        var optionNumber = 1;
        _voiceCombatOptions.Options.Clear();
        foreach (var move in _currentNpcMoves)
        {
            var optionButton = new VoiceCombatOptionButton(optionNumber, move.TextSid);
            optionButton.OnClick += (_, _) =>
            {
                UseMove(move, _rightCombatant);

                HandleCpuTurn();

                SelectNewOptionSet();

                CreateOptionControls();
            };

            _voiceCombatOptions.Options.Add(optionButton);
            optionNumber++;
        }
    }

    private void UseMove(VoiceCombatMove move, VoiceCombatant targetCombatant)
    {
        var voiceCombatantStatType = move.Damage.TargetStat;
        var statValue = targetCombatant.Stats[voiceCombatantStatType];
        var resultStateValue = statValue - move.Damage.Value;
        targetCombatant.Stats[voiceCombatantStatType] = resultStateValue;
    }

    private void HandleCpuTurn()
    {
        if (_intentedCpuMove is not null)
        {
            UseMove(_intentedCpuMove, _leftCombatant);
        }
        
        IntentCpuMove();
    }

    private void SelectNewOptionSet()
    {
        _currentNpcMoves.Clear();

        var hand = _dice.RollFromList(_availableHeroMoves, HERO_OPTION_COUNT);

        foreach (var item in hand)
        {
            _currentNpcMoves.Add(item);
        }        
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_campaign));
    }

    protected override void DrawHud(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        DrawSpeakerPortrait(spriteBatch, _leftCombatant.Sid, contentRectangle, CombatantPositionSide.Left);
        DrawSpeakerPortrait(spriteBatch, _rightCombatant.Sid, contentRectangle, CombatantPositionSide.Right);

        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), _leftCombatant.Stats[VoiceCombatantStatType.Conviction].ToString(), new Vector2(contentRectangle.Left, contentRectangle.Top), Color.White);

        spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), _rightCombatant.Stats[VoiceCombatantStatType.Conviction].ToString(), new Vector2(contentRectangle.Right - 50, contentRectangle.Top), Color.White);

        _voiceCombatOptions.Rect = new Rectangle(SPEAKER_FRAME_SIZE, contentRectangle.Bottom - _voiceCombatOptions.GetHeight() - 100,
                contentRectangle.Width - SPEAKER_FRAME_SIZE * 2,
                _voiceCombatOptions.GetHeight());
        _voiceCombatOptions.Draw(spriteBatch);

        spriteBatch.End();
    }
}
