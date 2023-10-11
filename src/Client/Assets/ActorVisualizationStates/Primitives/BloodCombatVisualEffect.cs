using System.Collections.Generic;
using System;

using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Particles;
//using MonoGame.Extended.Particles.Modifiers.Containers;
//using MonoGame.Extended.Particles.Modifiers.Interpolators;
//using MonoGame.Extended.Particles.Modifiers;
//using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;

namespace Client.Assets.ActorVisualizationStates.Primitives;

internal sealed class BloodCombatVisualEffect : ICombatVisualEffect
{
    private readonly IAnimationFrameSet _animation;
    private readonly Texture2D _bloodTexture;
    private readonly bool _flipX;
    private readonly Vector2 _position;

    //private ParticleEffect? _particleEffect;

    public BloodCombatVisualEffect(Vector2 position, Texture2D bloodTexture, bool flipX, IAnimationFrameSet animation)
    {
        _position = position;
        _bloodTexture = bloodTexture;
        _flipX = flipX;
        _animation = animation;
        _animation.End += (_, _) => {
            IsDestroyed = true;
            //_particleEffect?.Dispose();
            //_particleEffect = null;
        };

        //TextureRegion2D textureRegion = new TextureRegion2D(bloodTexture);
        //_particleEffect = new ParticleEffect { 
        //    Position = position,
        //    Emitters = new List<ParticleEmitter>
        //    {
        //        new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(2.5),
        //            Profile.BoxUniform(100,250))
        //        {
        //            Parameters = new ParticleReleaseParameters
        //            {
        //                Speed = new Range<float>(0f, 50f),
        //                Quantity = 3,
        //                Rotation = new Range<float>(-1f, 1f),
        //                Scale = new Range<float>(3.0f, 4.0f)
        //            },
        //            Modifiers =
        //            {
        //                new AgeModifier
        //                {
        //                    Interpolators =
        //                    {
        //                        new ColorInterpolator
        //                        {
        //                            StartValue = new HslColor(0.33f, 0.5f, 0.5f),
        //                            EndValue = new HslColor(0.5f, 0.9f, 1.0f)
        //                        }
        //                    }
        //                },
        //                new RotationModifier {RotationRate = -2.1f},
        //                new RectangleContainerModifier {Width = 800, Height = 480},
        //                new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
        //            }
        //        }
        //    }
        //};
    }

    public bool IsDestroyed { get; private set; }

    public void DrawBack(SpriteBatch spriteBatch)
    {
        // Draw nothing
    }

    public void DrawFront(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_bloodTexture, _position, _animation.GetFrameRect(), Color.White, 0, Vector2.Zero, 1,
            _flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        //if (_particleEffect is not null)
        //    spriteBatch.Draw(_particleEffect);
    }

    public void Update(GameTime gameTime)
    {
        if (IsDestroyed)
        {
            return;
        }

        _animation.Update(gameTime);
        //_particleEffect?.Update(gameTime.GetElapsedSeconds());
    }
}