namespace Core.Minigames.Towers;

public sealed class TowerRingMovedEventArgs: EventArgs
    {
        public TowerRingMovedEventArgs(TowerBar sourceBar, TowerBar targetBar)
        {
            SourceBar = sourceBar;
            TargetBar = targetBar;
        }

        public TowerBar SourceBar { get; }
        public TowerBar TargetBar { get; }
    }