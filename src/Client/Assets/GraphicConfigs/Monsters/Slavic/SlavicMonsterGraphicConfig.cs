﻿using Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters.Slavic;

internal abstract class SlavicMonsterGraphicConfig : MonsterGraphicsConfig
{
    protected SlavicMonsterGraphicConfig(UnitName unit) : base(unit)
    {
    }

    protected override CharacterCultureSid CultureSid => CharacterCultureSid.Slavic;
}