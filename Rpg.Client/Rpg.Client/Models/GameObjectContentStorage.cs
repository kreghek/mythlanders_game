using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Models
{
    internal class GameObjectContentStorage
    {
        private Texture2D _biomClouds;
        private Texture2D[] _combatBackgrounds;
        private Texture2D _combatUnitMarkers;
        private SpriteFont _font;
        private Texture2D? _mapNodes;
        private SoundEffect _monsterHit;
        private Texture2D? _monsterUnit;
        private SoundEffect _swordHit;
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

            _swordHit = contentManager.Load<SoundEffect>("Audio/GameObjects/SwordHit");
            _monsterHit = contentManager.Load<SoundEffect>("Audio/GameObjects/WolfHitEffect");

            _font = contentManager.Load<SpriteFont>("Fonts/Main");

            _combatBackgrounds = new Texture2D[]
            {
                contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/CloudsLayer"),
                contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/FarLayer"),
                contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/MainLayer"),
                contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/ClosestLayer")
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
            if (sid == "Player")
            {
                return _swordHit;
            }

            return _monsterHit;
        }

        internal Texture2D GetNodeMarker()
        {
            return _mapNodes;
        }
    }
}