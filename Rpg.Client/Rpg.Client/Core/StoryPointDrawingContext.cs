using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Core
{
    internal sealed class StoryPointDrawingContext : IStoryPointDrawingContext
    {
        public Rectangle? ResultRectangle { get; set; }
        public SpriteFont StoryJobsFont { get; init; }
        public SpriteFont StoryTitleFont { get; init; }
        public Rectangle TargetRectangle { get; init; }
        public SpriteBatch TargetSpriteBatch { get; init; }
    }
}