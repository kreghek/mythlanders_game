using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class UnitGraphics : UnitGraphicsBase
{
    private readonly bool _isNormalOrientation;
    private readonly UnitName _spriteSheetId;

    public UnitGraphics(UnitName spriteSheetId, UnitGraphicsConfigBase graphicsConfig, bool isNormalOrientation,
        Vector2 position, GameObjectContentStorage gameObjectContentStorage) :
        base(spriteSheetId, graphicsConfig, isNormalOrientation, position, gameObjectContentStorage)
    {
        _spriteSheetId = spriteSheetId;
        _isNormalOrientation = isNormalOrientation;
    }

    public bool IsDamaged { get; set; }

    public void ChangePosition(Vector2 position)
    {
        _position = position;
        InitializeSprites(_spriteSheetId, _isNormalOrientation);
    }
}