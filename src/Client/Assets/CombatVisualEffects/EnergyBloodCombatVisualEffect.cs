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

internal sealed class EnergyBloodCombatVisualEffect : ICombatVisualEffect
{
    private readonly Duration _duration;

    private readonly ParticleEffect _particleEffect;

    private double _lifetimeCounter;

    public EnergyBloodCombatVisualEffect(Vector2 position, HitDirection direction, TextureRegion2D bloodParticleTexture)
    {
        _duration = new Duration(0.05f);

        _particleEffect = new ParticleEffect
        {
            Position = position,
            Emitters = new List<ParticleEmitter>
            {
                new(bloodParticleTexture, 500, TimeSpan.FromSeconds(0.75),
                    Profile.Spray(direction == HitDirection.Right ? Vector2.UnitX : -Vector2.UnitX, 1))
                {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(100f, 250f),
                        Quantity = 30,
                        Rotation = new Range<float>(-1f, 1f),
                        Scale = new Range<float>(1.0f, 4.0f),
                        Color = new Range<HslColor>(HslColor.FromRgb(Color.Cyan),
                            HslColor.FromRgb(new Color(Color.LightCyan, 0.5f)))
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
                        new RotationModifier { RotationRate = -2.1f },
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