﻿using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal interface ISkillPanelState
    {
        public CombatSkill? SelectedCard { get; }
    }
}