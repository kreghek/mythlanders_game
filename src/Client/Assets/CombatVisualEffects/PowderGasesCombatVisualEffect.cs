using System;
using System.Collections.Generic;
using System.Linq;

using GameClient.Engine;
using GameClient.Engine.CombatVisualEffects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;

namespace Client.Assets.CombatVisualEffects;

internal sealed class PowderGasesCombatVisualEffect : ICombatVisualEffect
{
    private readonly Duration _duration;

    private readonly ParticleEffect _particleEffect;

    private double _lifetimeCounter;

    public PowderGasesCombatVisualEffect(Vector2 position, Vector2 targetPosition, TextureRegion2D sparksParticleTexture)
    {
        _duration = new Duration(0.05f);

        var direction = (targetPosition - position).NormalizedCopy();

        _particleEffect = new ParticleEffect
        {
            Position = position,
            Emitters = new List<ParticleEmitter>
            {
                new(sparksParticleTexture, 500, TimeSpan.FromSeconds(0.25),
                    Profile.Spray(direction, 0.1f))
                {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(400f, 1450f),
                        Quantity = 15,
                        Scale = new Range<float>(1f/48f, 1f/32f),
                        Color = new Range<HslColor>(HslColor.FromRgb(Color.Yellow),
                            HslColor.FromRgb(new Color(Color.Yellow, 0.5f)))
                    },
                    Modifiers =
                    {
                        new AgeModifier
                        {
                            Interpolators =
                            {
                                new OpacityInterpolator
                                {
                                    StartValue = 1,
                                    EndValue = 0.5f
                                }
                            }
                        },
                        new LinearGravityModifier { Direction = Vector2.UnitY, Strength = 250f }
                    }
                }
            }
        };
    }

    public bool IsDestroyed { get; private set; }

    public void DrawBack(SpriteBatch spriteBatch)
    {
        // Draw nothing
    }

    public void DrawFront(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_particleEffect);
    }

    public void Update(GameTime gameTime)
    {
        if (IsDestroyed)
        {
            return;
        }

        if (_lifetimeCounter >= _duration.Seconds)
        {
            _particleEffect.Emitters.First().AutoTrigger = false;

            if (_particleEffect.ActiveParticles == 0)
            {
                IsDestroyed = true;
                _particleEffect.Dispose();
            }
        }
        else
        {
            _lifetimeCounter += gameTime.GetElapsedSeconds();
        }

        _particleEffect.Update(gameTime.GetElapsedSeconds());
    }
}