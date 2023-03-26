using System;

using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;

namespace Rpg.Client.Assets.Equipments.Liberator
{
    internal sealed class VoiceModulator : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Liberator;
        public override EquipmentSid Sid => EquipmentSid.VoiceModulator;

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            throw new InvalidOperationException();
        }
    }
}