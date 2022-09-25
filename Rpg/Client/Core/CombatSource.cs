namespace Rpg.Client.Core
{
    internal class CombatSource
    {
        public CombatSource()
        {
            Level = 1;
        }

        public Group EnemyGroup { get; set; }
        public bool IsBossLevel { get; internal set; }
        public int Level { get; internal set; }
    }
}