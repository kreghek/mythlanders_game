using System;
using System.Collections.Generic;
using System.Linq;

using Client.Engine;
using Client.GameScreens;

using GameClient.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;

namespace Client.Assets.CombatVisualEffects;

internal sealed class ShieldCombatVisualEffect : ICombatVisualEffect
{
    private readonly Duration _duration;

    private readonly ParticleEffect _particleEffect;

    private double _lifetimeCounter;

    public ShieldCombatVisualEffect(Vector2 position, HitDirection direction, TextureRegion2D shieldParticleTexture,
        int combatantRadius)
    {
        _duration = new Duration(0.05f);

        _particleEffect = new ParticleEffect
        {
            Position = position,
            Emitters = new List<ParticleEmitter>
            {
                new(shieldParticleTexture, 500, TimeSpan.FromSeconds(0.5),
                    Profile.Spray(direction == HitDirection.Left ? Vector2.UnitX : -Vector2.UnitX, 3))
                {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(150f, 250f),
                        Quantity = 30,
                        Rotation = new Range<float>(-1f, 1f),
                        Scale = new Range<float>(0.25f, 0.5f),
                        Color = new Range<HslColor>(HslColor.FromRgb(TestamentColors.MainSciFi),
                            HslColor.FromRgb(new Color(TestamentColors.MainSciFi, 0.5f)))
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
                        },
                        new LinearGravityModifier { Direction = Vector2.UnitY, Strength = 250f },
                        new CircleContainerModifier
                        {
                            Radius = combatantRadius * 1.1f,
                            Inside = true,
                            RestitutionCoefficient = 0.2f
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