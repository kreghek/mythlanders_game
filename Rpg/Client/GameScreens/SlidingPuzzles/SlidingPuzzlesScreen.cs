using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Client.Core.Minigames.BarleyBreak;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Campaign;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.SlidingPuzzles
{

    internal sealed class SlidingPuzzlesScreen : GameScreenWithMenuBase
    {
        private const int BUTTON_SIZE = 75;
        private readonly HeroCampaign _campaign;
        private readonly SlidingPuzzlesEngine _puzzleEngine;
        private readonly int _width;
        private readonly int _height;
        private readonly SlidingPuzzlesMatrix _matrix;
        private readonly PuzzleButton[,] _buttonMatrix;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
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

            _buttonMatrix = new PuzzleButton[_width, _height];

            _gameObjectContentStorage = game.Services.GetRequiredService<GameObjectContentStorage>();
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
                var destinationRectangle = new Rectangle(contentRect.Left, contentRect.Top, BUTTON_SIZE * _width, BUTTON_SIZE * _height);
                spriteBatch.Draw(_gameObjectContentStorage.GetPuzzleTexture(), destinationRectangle, Color.White);
            }
            else
            {
                for (int x = 0; x < _width; x++)
                {
                    for (int y = 0; y < _height; y++)
                    {
                        var button = _buttonMatrix[x, y];

                        button.Number = _matrix[x, y];
                        button.Rect = new Rectangle(contentRect.Left + x * BUTTON_SIZE, contentRect.Top + y * BUTTON_SIZE, BUTTON_SIZE, BUTTON_SIZE);
                        button.Draw(spriteBatch);
                    }
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
                    var button = new PuzzleButton(_gameObjectContentStorage.GetPuzzleTexture(), _matrix.Width);

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
