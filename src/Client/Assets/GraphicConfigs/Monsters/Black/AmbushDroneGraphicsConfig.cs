using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Assets.CombatVisualEffects;
using Client.Core;
using Client.GameScreens;

using GameClient.Engine.Animations;
using GameClient.Engine.CombatVisualEffects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.TextureAtlases;

namespace Client.Assets.GraphicConfigs.Monsters.Black;

internal sealed class AmbushDroneGraphicsConfig : BlackMonsterGraphicConfig
{
    private SoundEffect _destructionSound = null!;
    private TextureRegion2D _damageParticleTexture = null!;

    public AmbushDroneGraphicsConfig(UnitName unit) : base(unit)
    {
        RemoveShadowOnDeath = true;
        Origin = new Vector2(100, 110);
    }

    public override void LoadContent(ContentManager contentManager)
    {
        _destructionSound = contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/AmbushDrone");

        var particleTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SfxObjects/Particles");
        _damageParticleTexture = new TextureRegion2D(particleTexture, new Rectangle(0, 32 * 3, 32, 32));
    }

    public override IDictionary<PredefinedAnimationSid, IAnimationFrameSet> GetPredefinedAnimations()
    {
        return new Dictionary<PredefinedAnimationSid, IAnimationFrameSet>
        {
            {
                PredefinedAnimationSid.Idle,
                AnimationFrameSetFactory.CreateIdle(fps: 8, frameCount: 4, textureColumns: 4, frameWidth: 128)
            },
            {
                PredefinedAnimationSid.MoveForward,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 0 }, fps: 8, textureColumns: 4,
                    frameWidth: 128)
            },
            {
                PredefinedAnimationSid.MoveBackward,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 0 }, fps: 8, textureColumns: 4,
                    frameWidth: 128)
            },
            {
                PredefinedAnimationSid.Wound,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 0 }, fps: 8, textureColumns: 4,
                    frameWidth: 128)
            },
            {
                PredefinedAnimationSid.Death,
                AnimationFrameSetFactory.CreateIdle(fps: 8, frameCount: 4, textureColumns: 4, frameWidth: 128)
            }
        };
    }

    public override IAnimationFrameSet GetDeathAnimation(GameObjectContentStorage gameObjectContentStorage, 
        ICombatVisualEffectManager combatVisualEffectManager,
        AudioSettings audioSettings,
        Vector2 position)
    {
        return new CombatVisualEffectAnimationFrameSet(new SoundedAnimationFrameSet(GetPredefinedAnimations()[PredefinedAnimationSid.Death], new AnimationFrame<IAnimationSoundEffect>[]
        {
            new AnimationFrame<IAnimationSoundEffect>(new AnimationFrameInfo(0), new AnimationSoundEffect(_destructionSound, audioSettings)),
            new AnimationFrame<IAnimationSoundEffect>(new AnimationFrameInfo(3), new AnimationSoundEffect(_destructionSound, audioSettings))
        }), combatVisualEffectManager,
        new[]
        {
            new AnimationFrame<ICombatVisualEffect>(new  AnimationFrameInfo(0), new MechanicalDamageVisualEffect(position, HitDirection.Right, _damageParticleTexture)),
            new AnimationFrame<ICombatVisualEffect>(new  AnimationFrameInfo(3), new MechanicalDamageVisualEffect(position, HitDirection.Right, _damageParticleTexture))
        });
    }
}