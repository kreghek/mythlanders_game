namespace Rpg.Client.Core
{
    public class Combat
    {
        public Group EnemyGroup { get; set; }
        public bool IsBossLevel { get; internal set; }
        public int Level { get; internal set; }
    }
}