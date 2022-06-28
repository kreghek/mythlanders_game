﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.InteractionDeliveryObjects
{
    internal class EnergoArrowBlast : IInteractionDelivery
    {
        private readonly IAnimationFrameSet _frameSet;
        private const float FPS = 8 * 2f;

        public EnergoArrowBlast(Vector2 position, Texture2D blastTexture)
        {
            _frameSet = AnimationFrameSetFactory.CreateSequential(7, frameCount: 7, fps: FPS, frameWidth: 64,
                frameHeight: 32, textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT, isLoop: false);

            _graphics = new Sprite(blastTexture)
            {
                Position = position,
                SourceRectangle = Rectangle.Empty
            };

            _frameSet.End += FrameSet_End;
        }

        private void FrameSet_End(object? sender, EventArgs e)
        {
            IsDestroyed = true;
            InteractionPerformed?.Invoke(this, EventArgs.Empty);
        }

        private readonly Sprite _graphics;

        protected virtual void DrawForegroundAdditionalEffects(SpriteBatch spriteBatch) { }

        protected virtual void UpdateAdditionalEffects(GameTime gameTime) { }

        public event EventHandler? InteractionPerformed;

        public bool IsDestroyed { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _graphics.Draw(spriteBatch);

            DrawForegroundAdditionalEffects(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (IsDestroyed)
            {
                return;
            }

            _frameSet.Update(gameTime);
            _graphics.SourceRectangle = _frameSet.GetFrameRect();

            UpdateAdditionalEffects(gameTime);
        }
    }
}