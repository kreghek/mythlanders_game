using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.ScreenManagement;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Challenge;

internal sealed class ChallengeScreen: GameScreenWithMenuBase
{
    private ResourceTextButton _acceptButton;
    private readonly Player _player;
    private readonly IReadOnlyCollection<IJob> _challengeJobs;
    private ResourceTextButton _skipButton;
    private readonly HeroCampaign _campaign;

    public ChallengeScreen(TestamentGame game, ChallengeScreenTransitionArguments args) : base(game)
    {
        var globeProvider = Game.Services.GetRequiredService<GlobeProvider>();

        _player = globeProvider.Globe.Player;
        _campaign = args.Campaign;
        _challengeJobs = args.Jobs;
    }

    protected override void InitializeContent()
    {
        _acceptButton = new ResourceTextButton(nameof(UiResource.AcceptChallengeButtonTitle));
        _acceptButton.OnClick += (_, _) =>
        {
            _player.Challenge = new CampaignChallenge(_challengeJobs, _player);

            ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
               new CampaignScreenTransitionArguments(_campaign));
        };

        _skipButton = new ResourceTextButton(nameof(UiResource.SkipButtonTitle));
        _skipButton.OnClick += (_, _) =>
        {
            ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                new CampaignScreenTransitionArguments(_campaign));
        };
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
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

        if (_player.Challenge?.CurrentJobs != null)
        {
            var currentJobsRect = new Rectangle(contentRect.Left + 200, contentRect.Top + 200,
                (contentRect.Width - 200 * 2), (contentRect.Height - 200 * 2) / 2);
            DrawCurrentChallengeJobs(spriteBatch, _player.Challenge.CurrentJobs, currentJobsRect);
        }

        var challengeJobsRect = new Rectangle(
            contentRect.Left + 200,
            contentRect.Center.Y + 50,
            (contentRect.Width - 200 * 2), 
            (contentRect.Height - 200 * 2) / 2 + 50);
        
        DrawCurrentChallengeJobs(spriteBatch, _challengeJobs, challengeJobsRect);

        _acceptButton.Rect = new Rectangle(challengeJobsRect.Center.X - 120, challengeJobsRect.Bottom + 20, 100, 20);
        _acceptButton.Draw(spriteBatch);
        
        _skipButton.Rect = new Rectangle(challengeJobsRect.Center.X + 120, challengeJobsRect.Bottom + 20, 100, 20);
        _skipButton.Draw(spriteBatch);

        spriteBatch.End();
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);
        
        _acceptButton.Update(ResolutionIndependentRenderer);
        _skipButton.Update(ResolutionIndependentRenderer);
    }

    private static void DrawCurrentChallengeJobs(SpriteBatch spriteBatch, IReadOnlyCollection<IJob> jobs, Rectangle contentRect)
    {
        var jobsArray = jobs.ToArray();
        for (var jobIndex = 0; jobIndex < jobsArray.Length; jobIndex++)
        {
            var job = jobsArray[jobIndex];
            var text = GetJobTypeDescription(job.Scheme.Type);
            var jobText = $"{text}: {job.Progress}/{job.Scheme.GoalValue.Value}";
            spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetTitlesFont(), jobText,
                new Vector2(contentRect.Left, contentRect.Top + jobIndex * 20), TestamentColors.MainSciFi);
        }
    }

    private static string GetJobTypeDescription(IJobType type)
    {
        if (type == JobTypeCatalog.Defeats)
        {
            return UiResource.JobTypeDefeat;
        }
        else if (type == JobTypeCatalog.Combats)
        {
            return UiResource.JobTypeCombats;
        }
        else if (type == JobTypeCatalog.CompleteCampaigns)
        {
            return UiResource.JobTypeCompleteCampans;
        }

        return "Unknown";
    }
}