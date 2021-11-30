using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class BiomeGenerator : IBiomeGenerator
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
            const int BIOME_MIN_LEVEL_STEP = 12;
            const int BIOME_NODE_COUNT = 8;

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
                },
                new Biome(BIOME_MIN_LEVEL_STEP, BiomeType.Chinese)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Chinese),
                            Sid = GetNodeSid(x, BiomeType.Chinese),
                            IsAvailable = GetStartAvailability(x)
                        }
                    ).ToArray(),
                    UnlockBiome = BiomeType.Egyptian
                },
                new Biome(BIOME_MIN_LEVEL_STEP * 2, BiomeType.Egyptian)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            Index = x, EquipmentItem = GetEquipmentItem(x, BiomeType.Egyptian),
                            Sid = GetNodeSid(x, BiomeType.Egyptian), IsAvailable = GetStartAvailability(x)
                        }
                    ).ToArray(),
                    UnlockBiome = BiomeType.Greek
                },
                new Biome(BIOME_MIN_LEVEL_STEP * 3, BiomeType.Greek)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            Index = x, EquipmentItem = GetEquipmentItem(x, BiomeType.Greek),
                            Sid = GetNodeSid(x, BiomeType.Greek), IsAvailable = GetStartAvailability(x)
                        }
                    ).ToArray(),
                    IsFinal = true
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