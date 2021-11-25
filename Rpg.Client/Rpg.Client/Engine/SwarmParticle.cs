using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal class SwarmParticle : IParticle
    {
        private readonly Rectangle _sourceRect;

        public SwarmParticle(Texture2D texture, Rectangle sourceRect, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl)
        {
            _sourceRect = sourceRect;
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
        }

        public float Angle { get; set; } // The current angle of rotation of the particle
        public float AngularVelocity { get; set; } // The speed that the angle is changing
        public Color Color { get; set; } // The color of the particle
        public Vector2 Position { get; set; } // The current position of the particle        
        public float Size { get; set; } // The size of the particle
        public Texture2D Texture { get; set; } // The texture that will be drawn to represent the particle
        public int TTL { get; set; } // The 'time to live' of the particle
        public Vector2 Velocity { get; set; } // The speed of the particle at the current instance

        public void Draw(SpriteBatch spriteBatch)
        {
            var origin = new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f);

            spriteBatch.Draw(Texture, Position, _sourceRect, Color,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }

        public void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;
        }
    }
}