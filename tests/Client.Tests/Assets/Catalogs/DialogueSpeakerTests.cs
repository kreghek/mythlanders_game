﻿using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

using FluentAssertions;

using NUnit.Framework;

namespace Client.Tests.Assets.Catalogs;

internal class DialogueSpeakerTests
{
    [Test]
    public void Equality_of_two_speakers_with_same_name()
    {
        // ARRANGE

        var speaker1 = new DialogueSpeaker(UnitName.Bogatyr);
        var speaker2 = new DialogueSpeaker(UnitName.Bogatyr);

        // ACT

        var isEquals = speaker1 == speaker2;

        // ASSERT

        isEquals.Should().BeTrue();
    }

    [Test]
    public void Equality_of_two_speakers_with_same_name_using_interfaces()
    {
        // ARRANGE

        IDialogueSpeaker speaker1 = new DialogueSpeaker(UnitName.Bogatyr);
        IDialogueSpeaker speaker2 = new DialogueSpeaker(UnitName.Bogatyr);

        // ACT

        var isEquals = speaker1.Equals(speaker2);

        // ASSERT

        isEquals.Should().BeTrue();
    }
}