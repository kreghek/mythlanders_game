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
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var button = _buttonMatrix[x, y];

                    button.Title = _matrix[x, y].ToString();
                    button.Rect = new Rectangle(contentRect.Left + x * 50, contentRect.Top + y * 50, 50, 50);
                    button.Draw(spriteBatch);
                }
            }
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

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
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var button = new TextButton2(_matrix[x, y].ToString());
                    button.OnClick += (_, _) =>
                    {
                        if (_puzzleEngine.TryMove(x, y))
                        { 

                        }
                    };
                    _buttonMatrix[x, y] = button;

                }
            }
        }
    }
}
