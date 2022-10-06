using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Client.Core.Minigames.BarleyBreak;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Campaign;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.SlidingPuzzles
{
    internal sealed class SlidingPuzzlesScreenTransitionArguments: IScreenTransitionArguments
    {
        public HeroCampaign Campaign { get; }

        public SlidingPuzzlesScreenTransitionArguments(HeroCampaign campaign)
        {
            Campaign = campaign;
        }
    }

    internal sealed class SlidingPuzzlesScreen : GameScreenWithMenuBase
    {
        private readonly HeroCampaign _campaign;
        private readonly SlidingPuzzlesEngine _puzzleEngine;
        private readonly int _width;
        private readonly int _height;
        private readonly SlidingPuzzlesMatrix _matrix;
        private readonly TextButton2[,] _buttonMatrix;
        private TimeSpan _currentTime;

        public SlidingPuzzlesScreen(EwarGame game, SlidingPuzzlesScreenTransitionArguments args) : base(game)
        {
            _campaign = args.Campaign;

            var matrix = new SlidingPuzzlesMatrix(new int[,] {
                { 5, 8, 0, 3 },
                { 2, 1, 11, 4 },
                { 9, 7, 6, 12 },
                { 13, 10, 14, 15 }
            });

            _puzzleEngine = new SlidingPuzzlesEngine(matrix);

            _width = matrix.Width;

            _height = matrix.Height;

            _matrix = matrix;

            _buttonMatrix = new TextButton2[_width, _height];
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var closeButton = new TextButton("Close");
            closeButton.OnClick += CloseButton_OnClick;

            return new[] {
                closeButton
            };
        }

        private void CloseButton_OnClick(object? sender, EventArgs e)
        {
            _campaign.CompleteCurrentStage();

            ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign, new CampaignScreenTransitionArguments
            {
                Campaign = _campaign
            });
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

            spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetTitlesFont(), "Ходов: " + _puzzleEngine.TurnCounter.ToString(), new Vector2(contentRect.Right - 200, contentRect.Top), Color.Wheat);
            spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetTitlesFont(), "Время: " + _currentTime.ToString(), new Vector2(contentRect.Right - 200, contentRect.Top + 100), Color.Wheat);

            if (_puzzleEngine.IsCompleted)
            {
                spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetTitlesFont(), "Победа!", new Vector2(contentRect.Right - 200, contentRect.Top + 200), Color.Wheat);
            }

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var button = _buttonMatrix[x, y];

                    button.Title = _matrix[x, y] == 0 ? string.Empty : _matrix[x, y].ToString();
                    button.Rect = new Rectangle(contentRect.Left + x * 50, contentRect.Top + y * 50, 50, 50);
                    button.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            if (_puzzleEngine.IsCompleted)
            {
                return;
            }

            _currentTime = _currentTime.Add(TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds));

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var button = _buttonMatrix[x, y];

                    button.Update(ResolutionIndependentRenderer);
                }
            }
        }

        protected override void InitializeContent()
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var button = new TextButton2(_matrix[x, y].ToString());

                    var currentX = x;
                    var currentY = y;

                    button.OnClick += (_, _) =>
                    {
                        if (_puzzleEngine.TryMove(currentX, currentY))
                        { 

                        }
                    };

                    _buttonMatrix[x, y] = button;
                }
            }
        }
    }
}
