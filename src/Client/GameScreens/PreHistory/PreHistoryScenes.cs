using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal static class PreHistoryScenes
{
    public static IDictionary<string, IPreHistoryScene> Create(ContentManager contentManager, Rectangle contentRect)
    {
        var backgrounds = new Dictionary<string, IPreHistoryScene>
        {
            {
                "AncientRising",
                new StaticScene(LoadTexture("AncientRising", contentManager), contentRect)
            },
            {
                "Monsters",
                new StaticScene(LoadTexture("Monsters", contentManager), contentRect)
            },
            {
                "MonstersAttack",
                new StaticScene(LoadTexture("MonstersAttack", contentManager), contentRect)
            },
            {
                "Hero",
                new StaticScene(LoadTexture("Hero", contentManager), contentRect)
            },
            {
                "FirstFraction",
                new FractionScene(
                    LoadTexture("SelectBlack", contentManager),
                    LoadTexture("SelectBlackDisabled", contentManager),
                    LoadTexture("SelectUnited", contentManager),
                    LoadTexture("SelectUnitedDisabled", contentManager))
            },
            {
                "Black",
                new StaticScene(LoadTexture("Black", contentManager), contentRect)
            },
            {
                "Union",
                new StaticScene(LoadTexture("Union", contentManager), contentRect)
            },
            {
                "StartHeroes",
                new StaticScene(LoadTexture("StartHeroes", contentManager), contentRect)
            },
            {
                "Monk",
                new StaticScene(LoadTexture("Monk", contentManager), contentRect)
            },
            {
                "Swordsman",
                new StaticScene(LoadTexture("Swordsman", contentManager), contentRect)
            },
            {
                "Hoplite",
                new StaticScene(LoadTexture("Hoplite", contentManager), contentRect)
            }
        };

        return backgrounds;
    }

    private static Texture2D LoadTexture(string fileName, ContentManager contentManager)
    {
        return contentManager.Load<Texture2D>(Path.Combine("Sprites","GameObjects","PreHistory", fileName));
    }
}
