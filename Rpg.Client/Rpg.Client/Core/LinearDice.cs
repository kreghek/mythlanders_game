using System;
using System.Diagnostics.CodeAnalysis;

namespace Rpg.Client.Core
{
    /// <summary>
    /// The implementation of the dice working by linear law.
    /// </summary>
    internal class LinearDice : IDice
    {
        private readonly Random _random;

        /// <summary>
        /// Конструктор кости.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public LinearDice()
        {
            _random = new Random();
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="seed"> The randomization seed. </param>
        /// <remarks>
        /// Same seeds make same random number sequence.
        /// </remarks>
        [ExcludeFromCodeCoverage]
        public LinearDice(int seed)
        {
            _random = new Random(seed);
        }

        /// <inheritdoc/>
        public int Roll(int n)
        {
            var rollResult = _random.Next(1, n + 1);
            return rollResult;
        }
    }
}
