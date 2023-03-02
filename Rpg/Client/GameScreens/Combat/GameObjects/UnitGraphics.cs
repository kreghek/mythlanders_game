using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class UnitGraphics : UnitGraphicsBase
{
    private readonly UnitName _spriteSheetId;
    private readonly bool _isNormalOrientation;

    public UnitGraphics(UnitName spriteSheetId, UnitGraphicsConfigBase graphicsConfig, bool isNormalOrientation, Vector2 position, GameObjectContentStorage gameObjectContentStorage) : 
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