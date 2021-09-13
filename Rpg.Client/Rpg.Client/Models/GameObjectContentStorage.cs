using System;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Models
{
    internal class GameObjectContentStorage
    {
        private Texture2D _combatUnitMarkers;
        private SpriteFont _font;
        private Texture2D? _mapNodes;
        private Texture2D? _monsterUnit;
        private Texture2D? _unit;
        private Texture2D _biomClouds;
        private SoundEffect _swordHit;

        public Texture2D GetUnitGraphics(string unitName)
        {
            switch (unitName)
            {
                case "Беримир":
                    return _unit;

                case "Рада":
                    return _unit;

                case "Соколинный глаз":
                    return _unit;

                default:
                    return _monsterUnit;
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            _unit = contentManager.Load<Texture2D>("Sprites/GameObjects/Unit");
            _monsterUnit = contentManager.Load<Texture2D>("Sprites/GameObjects/Wolf");
            _mapNodes = contentManager.Load<Texture2D>("Sprites/GameObjects/MapNodes");
            _combatUnitMarkers = contentManager.Load<Texture2D>("Sprites/GameObjects/CombatUnitMarkers");
            _biomClouds = contentManager.Load<Texture2D>("Sprites/GameObjects/Clouds");

            _swordHit = contentManager.Load<SoundEffect>("Audio/GameObjects/SwordHit");

            _font = contentManager.Load<SpriteFont>("Fonts/Main");
        }

        internal Texture2D GetBulletGraphics()
        {
            return _mapNodes;
        }

        internal Texture2D GetCombatUnitMarker()
        {
            return _combatUnitMarkers;
        }

        internal SpriteFont GetFont()
        {
            return _font;
        }

        internal Texture2D GetNodeMarker()
        {
            return _mapNodes;
        }

        internal Texture2D GetBiomeClouds()
        {
            return _biomClouds;
        }

        internal SoundEffect GetHitSound(string sid)
        {
            return _swordHit;
        }
    }
}