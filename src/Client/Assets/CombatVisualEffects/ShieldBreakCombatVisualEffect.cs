using System;
using System.Collections.Generic;
using System.Linq;

using Client.GameScreens;

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

internal sealed class ShieldBreakCombatVisualEffect : ICombatVisualEffect
{
    private readonly Duration _duration;

    private readonly ParticleEffect _particleEffect;

    private double _lifetimeCounter;

    public ShieldBreakCombatVisualEffect(Vector2 position, HitDirection direction,
        TextureRegion2D shieldParticleTexture)
    {
        _duration = new Duration(0.05f);

        _particleEffect = new ParticleEffect
        {
            Position = position,
            Emitters = new List<ParticleEmitter>
            {
                new(shieldParticleTexture, 500, TimeSpan.FromSeconds(0.5),
                    Profile.Spray(direction == HitDirection.Right ? Vector2.UnitX : -Vector2.UnitX, 3))
                {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(350f, 550f),
                        Quantity = 15,
                        Rotation = new Range<float>(-1f, 1f),
                        Scale = new Range<float>(0.25f, 0.5f),
                        Color = new Range<HslColor>(HslColor.FromRgb(MythlandersColors.MainSciFi),
                            HslColor.FromRgb(new Color(MythlandersColors.MainSciFi, 0.5f)))
                    },
                    Modifiers =
                    {
                        new AgeModifier
                        {
                            Interpolators =
                            {
                                new OpacityInterpolator
                                {
                                    StartValue = 0.75f,
                                    EndValue = 0f
                                }
                            }
                        }
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