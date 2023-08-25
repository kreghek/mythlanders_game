using Microsoft.Xna.Framework;

namespace Client.Engine;

public sealed class LayerSpeedCalculator
{
    public static Vector2[] CalculateSpeeds(Vector2[] speeds, int baseLayerIndex)
    {
        var calculatedSpeeds = new Vector2[speeds.Length];

        calculatedSpeeds[baseLayerIndex] = speeds[baseLayerIndex];

        for (var i = baseLayerIndex - 1; i >= 0; i--)
        {
            calculatedSpeeds[i] = calculatedSpeeds[i + 1] + speeds[i];
        }

        return calculatedSpeeds;
    }
}
