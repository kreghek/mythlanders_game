namespace Rpg.Client.Core
{
    /// <summary>
    /// The random number generator - dice.
    /// </summary>
    internal interface IDice
    {
        /// <summary>
        /// Get roll of the dice. Min is 1.
        /// </summary>
        /// <param name="n"> Edges of the dice. </param>
        /// <returns> The roll result. </returns>
        int Roll(int n);
    }
}