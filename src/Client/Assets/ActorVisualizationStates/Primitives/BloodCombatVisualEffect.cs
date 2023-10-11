using System.Collections.Generic;
using System;
using System.Linq;

using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;

namespace Client.Assets.ActorVisualizationStates.Primitives;

internal enum HitDirection
{
    Left,
    Right
}

internal sealed class BloodCombatVisualEffect : ICombatVisualEffect
{
    private readonly Duration _duration;
    
    private readonly Texture2D _bloodParticleTexture;
    private readonly Vector2 _position;
    private readonly HitDirection _direction;

    private readonly ParticleEffect _particleEffect;

    public BloodCombatVisualEffect(Vector2 position, HitDirection direction, Texture2D bloodParticleTexture)
    {
        _duration = new Duration(0.05f);
        
        _position = position;
        _direction = direction;
        _bloodParticleTexture = bloodParticleTexture;
        

        TextureRegion2D textureRegion = new TextureRegion2D(bloodParticleTexture);
        _particleEffect = new ParticleEffect { 
            Position = position,
            Emitters = new List<ParticleEmitter>
            {
                new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(0.5),
                    Profile.Spray(_direction == HitDirection.Left ? Vector2.UnitX : -Vector2.UnitX, 1))
                {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(0f, 50f),
                        Quantity = 3,
                        Rotation = new Range<float>(-1f, 1f),
                        Scale = new Range<float>(3.0f, 4.0f)
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
                                    EndValue = 0.25f
                                }
                            }
                        },
                        new RotationModifier {RotationRate = -2.1f},
                        new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 130f},
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

    private double _lifetimeCounter;

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