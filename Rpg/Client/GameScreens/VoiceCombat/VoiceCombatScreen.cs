using System;
using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;
using Client.Core.Dialogues;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.GameScreens.TextDialogue.Ui;

using Core.Dices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Speech.Ui;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.VoiceCombat;

internal class VoiceCombatScreen : DialogueScreenBase
{
    private class VoiceCombatant
    {
        public int Hp { get; set; }
        public UnitName Sid { get; }

        public VoiceCombatant(int hp, UnitName sid)
        {
            Hp = hp;
            Sid = sid;
        }
    }

    private readonly HeroCampaign _campaign;

    private readonly VoiceCombatant _leftCombatant;
    private readonly VoiceCombatant _rightCombatant;

    private readonly VoiceCombatOptions _voiceCombatOptions;

    public VoiceCombatScreen(TestamentGame game, VoiceCombatScreenTransitionArguments args) : base(game, args.Campaign)
    {
        _campaign = args.Campaign;

        _leftCombatant = new VoiceCombatant(13, UnitName.Assaulter);
        _rightCombatant = new VoiceCombatant(10, UnitName.ChineseOldman);

        _voiceCombatOptions = new VoiceCombatOptions();

        _heroMoves = new List<VoiceCombatMove> { new VoiceCombatMove("Alert!"), new VoiceCombatMove("TheGoodWeather") };
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var closeButton = new ResourceTextButton(nameof(UiResource.SkipButtonTitle));
        closeButton.OnClick += CloseButton_OnClick;

        return new[]
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
        public VoiceCombatMove(string textSid, VoiceCombatMoveType type, IReadOnlyCollection<IVoiceCombatMoveEffect> effects)
        {
            TextSid = textSid;
            Type = type;
            Effects = effects;
        }

        public string TextSid { get; }

        public VoiceCombatMoveType Type { get; }
        public IReadOnlyCollection<IVoiceCombatMoveEffect> Effects { get; }
    }

    private enum VoiceCombatMoveType
    {
        Diplomacy,
        Domination,
        Sophistic
    }

    private interface IVoiceCombatMoveEffect
    {

    }

    private interface IVoiceCombatMoveEffectTargetSelector
    {

    }

    private sealed class VoiceCombatMoveInstace
    {

    }

    private readonly IList<VoiceCombatMove> _heroMoves;

    private void InitDialogueControls()
    {

        var optionNumber = 1;
        _voiceCombatOptions.Options.Clear();
        foreach (var move in _heroMoves)
        {
            var optionButton = new VoiceCombatOptionButton(optionNumber, move.TextSid);
            optionButton.OnClick += (_, _) =>
            {
                
            };

            _voiceCombatOptions.Options.Add(optionButton);
            optionNumber++;
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

        _voiceCombatOptions.Rect = new Rectangle(SPEAKER_FRAME_SIZE, contentRectangle.Bottom - _voiceCombatOptions.GetHeight() - 100,
                contentRectangle.Width - SPEAKER_FRAME_SIZE * 2,
                _voiceCombatOptions.GetHeight());
        _voiceCombatOptions.Draw(spriteBatch);

        spriteBatch.End();
    }
}
