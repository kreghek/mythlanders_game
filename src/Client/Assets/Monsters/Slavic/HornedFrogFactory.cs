using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class HornedFrogFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.HornedFrog;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;
}