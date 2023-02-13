﻿using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal interface IUiContentStorage
    {
        bool ContentWasLoaded { get; }

        Texture2D GetBiomeBackground(BiomeType type);
        Texture2D GetButtonIndicatorsTexture();
        SpriteFont GetCombatIndicatorFont();
        Texture2D GetCombatPowerIconsTexture();
        Texture2D GetCombatSkillPanelTexture();
        IReadOnlyCollection<CombatSoundtrack> GetCombatSongs(BiomeType currentBiome);
        Texture2D GetControlBackgroundTexture();
        Texture2D GetCursorsTexture();
        Song GetDefeatSong();
        Texture2D GetDisabledTexture();

        Texture2D GetEffectIconsTexture();
        Texture2D GetEquipmentTextures();
        Song GetIntroSong();
        Texture2D[] GetIntroVideo();
        Texture2D GetLogoTexture();
        SpriteFont GetMainFont();
        IEnumerable<Song> GetMapSong();
        Texture2D[] GetModalBottomTextures();
        Texture2D GetModalShadowTexture();
        Texture2D GetModalTopSymbolTextures();
        Texture2D[] GetModalTopTextures();
        Texture2D GetSocialTexture();
        Texture2D GetTitleBackgroundTexture();
        SpriteFont GetTitlesFont();
        Song GetTitleSong();
        Texture2D GetUnitStatePanelTexture();
        Song GetVictorySong();
        void LoadContent(ContentManager contentManager);
    }
}