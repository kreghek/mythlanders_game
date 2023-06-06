using System;

using Client.Engine;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.Assets.InteractionDeliveryObjects;

internal sealed class SvarogSymbolObject : IInteractionDelivery
{
    private const double FRAMERATE = 1f / 8f;
    private const int SYMBOL_SIZE = 128;
    private const int SYMBOL_COL_COUNT = 4;

    private const int APPEARING_FRAME_COUNT = 9;

    // -1 to concert count to index
    // +1 to move index to next frame
    private const int BURNING_START_FRAME_INDEX = APPEARING_FRAME_COUNT - 1 + 1;
    private const int BURNING_FRAME_COUNT = 8;
    private const int BURNING_FRAME_LAST_INDEX = BURNING_START_FRAME_INDEX + BURNING_FRAME_COUNT;
    private const int BURNING_LOOP_START_FRAME_INDEX = 14;
    private const double EXPLOSION_DELAY_DURATION_SECONDS = 1.5;
    private const double EXPLOSION_AFTER_DELAY_DURATION_SECONDS = 1.5;
    private const int BURNING_LOOP_ITERATION_COUNT = 4;


    private readonly AnimationBlocker _animationBlocker;
    private readonly GameObjectContentStorage _contentStorage;
    private readonly Sprite _graphics;
    private readonly Action _interaction;
    private readonly Vector2 _targetPosition;

    private bool _appearingEventRaised;

    private double _explosionAfterDelayCounterSeconds;

    private double _explosionDelayCounterSeconds;

    private bool _explosionExecuted;
    private ParticleSystem? _explosionParticleSystem;

    private bool _explosionParticleSystemStarted;

    private int _finalBurningIterations;
    private double _frameCounter;
    private int _frameIndex;

    private bool _risingPowerEventRaised;
    private ParticleSystem? _risingPowerParticleSystem;

    private bool _risingPowerParticleSystemIsStarted;

    private int _stageIndex;

    public SvarogSymbolObject(Vector2 targetPosition, GameObjectContentStorage contentStorage,
        AnimationBlocker animationBlocker, Action interaction)
    {
        _graphics = new Sprite(contentStorage.GetSymbolSprite());

        _targetPosition = targetPosition;
        _contentStorage = contentStorage;
        _animationBlocker = animationBlocker;
        _interaction = interaction;
        _graphics.Position = _targetPosition;
        _graphics.SourceRectangle = new Rectangle(0, 0, SYMBOL_SIZE, SYMBOL_SIZE);
    }

    public void SwitchStage(int stageIndex)
    {
        _stageIndex = stageIndex;
        _frameCounter = 0;
    }

    private void HandleAppearingStage(GameTime gameTime)
    {
        if (_frameCounter >= FRAMERATE)
        {
            _frameCounter = 0;

            if (_frameIndex < APPEARING_FRAME_COUNT - 1)
            {
                _frameIndex++;
            }
            else
            {
                if (!_appearingEventRaised)
                {
                    _appearingEventRaised = true;
                    AppearingCompleted?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        else
        {
            _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    private void HandleExplodingStage(GameTime gameTime)
    {
        _risingPowerParticleSystem?.Stop();

        if (!_explosionParticleSystemStarted)
        {
            var explosionParticleGenerator =
                new ExplosionParticleGenerator(new[] { _contentStorage.GetParticlesTexture() },
                    new Rectangle(0, 64, 32, 32));
            _explosionParticleSystem = new ParticleSystem(_targetPosition, explosionParticleGenerator);

            _explosionParticleSystemStarted = true;
        }
        else
        {
            if (_explosionParticleSystem is null)
            {
                throw new InvalidOperationException(
                    "The particle system should be assigned as start of the stage.");
            }

            _explosionParticleSystem.Update(gameTime);
        }

        if (!_explosionExecuted)
        {
            _explosionDelayCounterSeconds += gameTime.ElapsedGameTime.TotalSeconds;

            if (_explosionDelayCounterSeconds >= EXPLOSION_DELAY_DURATION_SECONDS)
            {
                _interaction.Invoke();
                _explosionExecuted = true;
                _explosionParticleSystem.Stop();
            }
        }
        else
        {
            _explosionAfterDelayCounterSeconds += gameTime.ElapsedGameTime.TotalSeconds;

            if (_explosionAfterDelayCounterSeconds >= EXPLOSION_AFTER_DELAY_DURATION_SECONDS)
            {
                _animationBlocker.Release();
                IsDestroyed = true;
                InteractionPerformed?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void HandleRisingPowerStage(GameTime gameTime)
    {
        if (!_risingPowerParticleSystemIsStarted)
        {
            _risingPowerParticleSystemIsStarted = true;

            var particleTextures = new[] { _contentStorage.GetParticlesTexture() };
            var particleGenerator = new MothParticleGenerator(particleTextures);
            _risingPowerParticleSystem = new ParticleSystem(_targetPosition, particleGenerator);
        }
        else
        {
            if (_risingPowerParticleSystem is null)
            {
                throw new InvalidOperationException(
                    "The particle system should be assigned as start of the stage.");
            }

            _risingPowerParticleSystem.Update(gameTime);
        }

        if (_frameIndex < BURNING_START_FRAME_INDEX)
        {
            _frameIndex = BURNING_START_FRAME_INDEX;
        }

        if (_frameCounter >= FRAMERATE)
        {
            _frameCounter = 0;
            _frameIndex++;

            if (_frameIndex > BURNING_FRAME_LAST_INDEX)
            {
                if (_finalBurningIterations < BURNING_LOOP_ITERATION_COUNT)
                {
                    _finalBurningIterations++;
                    _frameIndex = BURNING_LOOP_START_FRAME_INDEX;
                }
                else if (!_risingPowerEventRaised)
                {
                    _risingPowerEventRaised = true;
                    RisingPowerCompleted?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        else
        {
            _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public bool IsDestroyed { get; private set; }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsDestroyed)
        {
            return;
        }

        _graphics.Draw(spriteBatch);

        _risingPowerParticleSystem?.Draw(spriteBatch);
        _explosionParticleSystem?.Draw(spriteBatch);
    }

    public void Update(GameTime gameTime)
    {
        if (IsDestroyed)
        {
            return;
        }

        _graphics.SourceRectangle = new Rectangle(SYMBOL_SIZE * (_frameIndex % SYMBOL_COL_COUNT),
            SYMBOL_SIZE * (_frameIndex / SYMBOL_COL_COUNT),
            SYMBOL_SIZE, SYMBOL_SIZE);

        if (_stageIndex == 0)
        {
            HandleAppearingStage(gameTime);
        }
        else if (_stageIndex == 1)
        {
            HandleRisingPowerStage(gameTime);
        }
        else if (_stageIndex == 2)
        {
            HandleExplodingStage(gameTime);
        }
    }

    public event EventHandler? InteractionPerformed;

    public event EventHandler? AppearingCompleted;

    public event EventHandler? RisingPowerCompleted;
}