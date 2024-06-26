﻿namespace Client.Core.EventSerialization;

internal sealed record EventStorageModel
{
    public string? AfterCombatAftermath { get; set; }
    public EventNodeStorageModel AfterCombatNode { get; set; }
    public string? BeforeCombatAftermath { get; set; }

    public EventNodeStorageModel BeforeCombatNode { get; set; }

    public string? GoalDescription { get; set; }
    public string Location { get; set; }
    public string Name { get; set; }
    public string[]? ParentSids { get; set; }
    public string Sid { get; set; }
}