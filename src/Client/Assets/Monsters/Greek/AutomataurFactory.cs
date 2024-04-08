using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Greek;

[UsedImplicitly]
internal class AutomataurFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Automataur;

    public override CharacterCultureSid Culture => CharacterCultureSid.Greek;
}