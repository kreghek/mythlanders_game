using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Core
{
    internal sealed class StoryPointDrawingContext : IStoryPointDrawingContext
    {
        public SpriteBatch TargetSpriteBatch { get; init; }
        public Rectangle TargetRectangle { get; init; }
        public Rectangle? ResultRectangle { get; set; }
        public SpriteFont StoryTitleFont { get; init; }
        public SpriteFont StoryJobsFont { get; init; }
    }
}