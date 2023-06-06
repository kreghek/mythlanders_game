using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

internal interface IParticle
{
    float Angle { get; set; } // The current angle of rotation of the particle
    float AngularVelocity { get; set; } // The speed that the angle is changing
    Color Color { get; set; } // The color of the particle
    Vector2 Position { get; set; } // The current position of the particle        
    float Size { get; set; } // The size of the particle
    Texture2D Texture { get; set; } // The texture that will be drawn to represent the particle
    int TTL { get; set; } // The 'time to live' of the particle
    Vector2 Velocity { get; set; } // The speed of the particle at the current instance
    void Draw(SpriteBatch spriteBatch);
    void Update();
}