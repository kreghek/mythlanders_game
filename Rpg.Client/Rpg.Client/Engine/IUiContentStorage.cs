﻿using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal interface IUiContentStorage
    {
        IEnumerable<Song> GetBattleSongs();
        Texture2D GetBiomeBackground(BiomeType type);
        Texture2D GetButtonTexture();
        Texture2D GetCombatPowerIconsTexture();
        SpriteFont GetMainFont();
        IEnumerable<Song> GetMapSong();

        Texture2D[] GetModalBottomTextures();

        Texture2D GetModalShadowTexture();

        Texture2D[] GetModalTopTextures();
        Song GetTitleSong();
        void LoadContent(ContentManager contentManager);
    }
}