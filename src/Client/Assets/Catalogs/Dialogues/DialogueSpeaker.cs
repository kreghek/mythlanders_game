using System.Collections;

using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.Dialogues;

public sealed record DialogueSpeaker(UnitName Unit): IDialogueSpeaker;