using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal interface IParticleGenerator
    {
        IParticle GenerateNewParticle(Vector2 emitterPosition);
    }
}