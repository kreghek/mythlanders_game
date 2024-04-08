using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class VampireFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Vampire;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;
}