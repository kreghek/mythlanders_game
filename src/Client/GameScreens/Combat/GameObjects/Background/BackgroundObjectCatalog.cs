using System.Collections.Generic;

using Client.Assets;
using Client.GameScreens;

using Microsoft.Xna.Framework;

using Rpg.Client.GameScreens.Combat.GameObjects.Background;

namespace Client.GameScreens.Combat.GameObjects.Background;

internal sealed class BackgroundObjectCatalog
{
    private readonly GameObjectContentStorage _gameObjectContentStorage;

    public BackgroundObjectCatalog(GameObjectContentStorage gameObjectContentStorage)
    {
        _gameObjectContentStorage = gameObjectContentStorage;
    }

    public IReadOnlyCollection<BackgroundObjectBase> GetObjects()
    {
        return new BackgroundObjectBase[]
        {
            new StaticBackgroundObject(
                _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(LocationTheme.ChineseMonastery,
                    BackgroundLayerType.Far, 0),
                new Rectangle(256 * 0, 0, 256, 256)),
            new StaticBackgroundObject(
                _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(LocationTheme.ChineseMonastery,
                    BackgroundLayerType.Far, 0), new Rectangle(256 * 1, 0, 256, 256)),
            new StaticBackgroundObject(
                _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(LocationTheme.ChineseMonastery,
                    BackgroundLayerType.Far, 0), new Rectangle(256 * 0, 256 * 1, 256, 256)),
            new StaticBackgroundObject(
                _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(LocationTheme.ChineseMonastery,
                    BackgroundLayerType.Far, 0), new Rectangle(256 * 1, 256 * 1, 256, 256))
        };
    }
}