using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class DemoBiomeGenerator: IBiomeGenerator
    {
        private static GlobeNodeSid GetNodeSid(int nodeIndex, BiomeType biomType)
        {
            var sidIndex = (int)biomType + nodeIndex + 1;
            var sid = (GlobeNodeSid)sidIndex;
            return sid;
        }
        
        private static bool GetStartAvailability(int nodeIndex)
        {
            return nodeIndex == 0;
        }
        
        public IReadOnlyList<Biome> Generate()
        {
            const int BIOME_NODE_COUNT = 4;

            return new[]
            {
                new Biome(0, BiomeType.Slavic)
                {
                    IsAvailable = true,
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Slavic),
                            Sid = GetNodeSid(x, BiomeType.Slavic),
                            IsAvailable = GetStartAvailability(x)
                        }
                    ).ToArray(),
                    UnlockBiome = BiomeType.Chinese,
                    IsStart = true
                }
            };
        }

        private static EquipmentItemType? GetEquipmentItem(int nodeIndex, BiomeType biomType)
        {
            switch (biomType)
            {
                case BiomeType.Slavic:
                    {
                        return nodeIndex switch
                        {
                            // TODO Rewrite to pattern matching with range
                            0 => EquipmentItemType.Warrior,
                            1 => EquipmentItemType.Archer,
                            2 => EquipmentItemType.Herbalist,
                            3 => EquipmentItemType.Warrior,
                            4 => EquipmentItemType.Archer,
                            5 => EquipmentItemType.Herbalist,

                            _ => null
                        };
                    }

                case BiomeType.Egyptian:
                    {
                        return nodeIndex switch
                        {
                            0 => EquipmentItemType.Priest,
                            3 => EquipmentItemType.Priest,
                            _ => null
                        };
                    }

                default:
                    return null;
            }
        }
    }
}