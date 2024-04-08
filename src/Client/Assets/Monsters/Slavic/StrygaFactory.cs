using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class StrygaFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Stryga;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;
}