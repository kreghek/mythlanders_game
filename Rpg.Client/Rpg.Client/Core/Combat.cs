namespace Rpg.Client.Core
{
    internal class Combat
    { 
        public Group EnemyGroup { get; set; }
        public bool IsBossLevel { get; internal set; }
        public int Level { get; internal set; }
    }
}
