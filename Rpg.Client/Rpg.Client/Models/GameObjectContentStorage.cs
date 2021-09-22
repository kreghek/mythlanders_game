﻿using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Models
{
    internal enum BackgroundType
    {
        Undefined,
        SlavicBattleground,
        SlavicSwamp
    }

    internal class GameObjectContentStorage
    {
        private Texture2D _biomClouds;
        private Dictionary<BackgroundType, Texture2D[]> _combatBackgroundDict;

        private Dictionary<string, SoundEffect> _combatPowerDict;
        private Texture2D _combatUnitMarkers;
        private SpriteFont _font;
        private Texture2D? _mapNodes;
        private Texture2D? _monsterUnit;
        private Texture2D? _unit;

        public Texture2D GetUnitGraphics(string unitName)
        {
            return unitName switch
            {
                "Беримир" => _unit,
                "Рада" => _unit,
                "Соколинный глаз" => _unit,
                _ => _monsterUnit
            };
        }

        public void LoadContent(ContentManager contentManager)
        {
            _unit = contentManager.Load<Texture2D>("Sprites/GameObjects/Unit");
            _monsterUnit = contentManager.Load<Texture2D>("Sprites/GameObjects/Wolf");
            _mapNodes = contentManager.Load<Texture2D>("Sprites/GameObjects/MapNodes");
            _combatUnitMarkers = contentManager.Load<Texture2D>("Sprites/GameObjects/CombatUnitMarkers");
            _biomClouds = contentManager.Load<Texture2D>("Sprites/GameObjects/Clouds");

            _font = contentManager.Load<SpriteFont>("Fonts/Main");

            _combatBackgroundDict = new Dictionary<BackgroundType, Texture2D[]>
            {
                {
                    BackgroundType.SlavicBattleground, new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/CloudsLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/FarLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/MainLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/ClosestLayer")
                    }
                },

                {
                    BackgroundType.SlavicSwamp, new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Swamp/CloudsLayer"),
                        contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/Slavic/Swamp/FarLayer"),
                        contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/Slavic/Swamp/MainLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Swamp/ClosestLayer")
                    }
                }
            };

            _combatPowerDict = new Dictionary<string, SoundEffect>
            {
                { "Slash", contentManager.Load<SoundEffect>("Audio/GameObjects/SwordHit") },
                { "Wide Slash", contentManager.Load<SoundEffect>("Audio/GameObjects/SwordHit") },
                { "Heal", contentManager.Load<SoundEffect>("Audio/GameObjects/HealEffect") },
                { "Dope Herb", contentManager.Load<SoundEffect>("Audio/GameObjects/DustEffect") },
                { "Mass Stun", contentManager.Load<SoundEffect>("Audio/GameObjects/EgyptMassStunEffect") },
                { "Strike", contentManager.Load<SoundEffect>("Audio/GameObjects/BowStrikeEffect") },
                { "Arrow Rain", contentManager.Load<SoundEffect>("Audio/GameObjects/BowStrikeEffect") },
                { "Power Up", contentManager.Load<SoundEffect>("Audio/GameObjects/HealEffect") },

                { "Wolf Bite", contentManager.Load<SoundEffect>("Audio/GameObjects/WolfHitEffect") }
            };
        }

        internal Texture2D GetBiomeClouds()
        {
            return _biomClouds;
        }

        internal Texture2D GetBulletGraphics()
        {
            return _mapNodes;
        }

        internal Texture2D[] GetCombatBackgrounds(BackgroundType backgroundType)
        {
            return _combatBackgroundDict[backgroundType];
        }

        internal Texture2D GetCombatUnitMarker()
        {
            return _combatUnitMarkers;
        }

        internal SpriteFont GetFont()
        {
            return _font;
        }

        internal SoundEffect GetHitSound(string sid)
        {
            if (_combatPowerDict.TryGetValue(sid, out var soundEffect))
            {
                return soundEffect;
            }

            return _combatPowerDict["Wolf Bite"];
        }

        internal Texture2D GetNodeMarker()
        {
            return _mapNodes;
        }
    }
}