using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Assets.InteractionDeliveryObjects;
using Client.Core;
using Client.Engine;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;

using GameClient.Engine.Animations;
using GameClient.Engine.MoveFunctions;

using Microsoft.Xna.Framework;

namespace Client.Assets.CombatMovements.Hero.Robber;

internal sealed class ArrowRainSourceInteractionDeliveryFactory : IDeliveryFactory
{
    private const int FPS = 8 * 3;

    private readonly GameObjectContentStorage _gameObjectContentStorage;

    public ArrowRainSourceInteractionDeliveryFactory(GameObjectContentStorage gameObjectContentStorage)
    {
        _gameObjectContentStorage = gameObjectContentStorage;
    }

    private static IAnimationFrameSet CreateFrameSet()
    {
        var rising = AnimationFrameSetFactory.CreateSequential(
            startFrameIndex: 4,
            frameCount: 5,
            fps: FPS,
            frameWidth: SfxSpriteConsts.Size64x32.WIDTH,
            frameHeight: SfxSpriteConsts.Size64x32.HEIGHT,
            textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT,
            isLoop: false);

        var body = AnimationFrameSetFactory.CreateSequential(
            startFrameIndex: 9,
            frameCount: 5,
            fps: FPS,
            frameWidth: SfxSpriteConsts.Size64x32.WIDTH,
            frameHeight: SfxSpriteConsts.Size64x32.HEIGHT,
            textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT,
            isLoop: true);

        var full = new CompositeAnimationFrameSet(rising, body);

        return full;
    }

    /// <inheritdoc />
    public IInteractionDelivery Create(CombatEffectImposeItem interactionImpose, Vector2 startPoint,
        Vector2 targetPoint)
    {
        return new Projectile(
            new ProjectileFunctions(new LinearMoveFunction(startPoint, targetPoint), new RotateNone()),
            _gameObjectContentStorage.GetBulletGraphics(),
            CreateFrameSet(),
            new Duration(1).Seconds);
    }
}