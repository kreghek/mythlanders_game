﻿namespace PlotConverter
{
    internal sealed record ExcelEventRow
    {
        public string BeforeCombatAftermath { get; init; }
        public string AfterCombatAftermath { get; init; }
        public string Location { get; init; }
        public string Name { get; init; }
        public string Sid { get; init; }
        public string GoalDescription { get; set; }
    }
}