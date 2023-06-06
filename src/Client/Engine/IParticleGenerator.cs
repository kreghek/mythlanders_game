using Microsoft.Xna.Framework;

namespace Client.Engine;

internal interface IParticleGenerator
{
    IParticle GenerateNewParticle(Vector2 emitterPosition);

    int GetCount();

    float GetTimeout();
}