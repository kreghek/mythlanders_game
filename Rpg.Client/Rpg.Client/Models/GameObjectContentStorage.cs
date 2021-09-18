using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Rpg.Client.Models
{
    internal class GameObjectContentStorage
    {
        private Texture2D _biomClouds;
        private Texture2D[] _combatBackgrounds;
        private Texture2D _combatUnitMarkers;
        private SpriteFont _font;
        private Texture2D? _mapNodes;
        private Texture2D? _monsterUnit;
        private Texture2D? _unit;
        
        private Dictionary<string, SoundEffect> _combatPowerDict;

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

            _combatBackgrounds = new Texture2D[]
            {
                contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/CloudsLayer"),
                contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/FarLayer"),
                contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/MainLayer"),
                contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/ClosestLayer")
            };
            
            _combatPowerDict = new Dictionary<string, SoundEffect>
            {
                { "Slash", contentManager.Load<SoundEffect>("Audio/GameObjects/SwordHit") },
                { "Wide Slash", contentManager.Load<SoundEffect>("Audio/GameObjects/SwordHit") },
                { "Heal", contentManager.Load<SoundEffect>("Audio/GameObjects/HealEffect") },
                { "Dope Herbs", contentManager.Load<SoundEffect>("Audio/GameObjects/DustEffect") },
                { "Bow Strike", contentManager.Load<SoundEffect>("Audio/GameObjects/BowStrikeEffect") },
                { "Arrow Rain", contentManager.Load<SoundEffect>("Audio/GameObjects/BowStrikeEffect") },
                
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

        internal Texture2D[] GetCombatBackgrounds()
        {
            return _combatBackgrounds;
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
            else
            {
                return _combatPowerDict["Wolf Bite"];
            }
        }

        internal Texture2D GetNodeMarker()
        {
            return _mapNodes;
        }
    }
}
