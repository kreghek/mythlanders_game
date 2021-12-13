using System.Collections.Generic;

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
        Song GetDefeatSong();
        Texture2D GetEnvSpeechTexture();
        SpriteFont GetMainFont();
        SpriteFont GetTitlesFont();
        IEnumerable<Song> GetMapSong();

        Texture2D[] GetModalBottomTextures();

        Texture2D GetModalShadowTexture();

        Texture2D[] GetModalTopTextures();
        Texture2D GetSpeechTexture();
        Song GetTitleSong();
        Texture2D GetUnitPanelTexture();
        Song GetVictorySong();
        void LoadContent(ContentManager contentManager);
    }
}