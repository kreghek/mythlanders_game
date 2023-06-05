using System;
using System.Linq;

using Client.Assets;
using Client.Assets.Dialogues;
using Client.Core.Dialogues;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Tests.Assets.Dialogues;

[TestFixture(Category = "Assets")]
public class SynthAsParentDialogueEventFactoryTests
{
    [Test]
    public void Get_stage_1_dialogue_on_start()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());

        // ACT

        var dialogueSid = dialogueEvent.GetDialogSid();

        // ASSERT

        dialogueSid.Should().Be("synth_as_parent_stage_1");
    }

    [Test]
    public void Get_canon_stage_2_dialogue_when_complete_stage_1_challenge()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());
        dialogueEvent.Trigger(DialogueConstants.SideQuests.SynthAsParent.Stage1_Repair_Trigger);
        dialogueEvent.Trigger(DialogueConstants.CompleteCurrentStageChallengeTrigger);

        // ACT

        var dialogueSid = dialogueEvent.GetDialogSid();

        // ASSERT

        dialogueSid.Should().Be("synth_as_parent_stage_2");
    }

    [Test]
    public void Get_fast_stage_2_dialogue_when_complete_stage_1_challenge()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());
        dialogueEvent.Trigger(DialogueConstants.SideQuests.SynthAsParent.Stage1_Extradite_Trigger);
        dialogueEvent.Trigger(DialogueConstants.CompleteCurrentStageChallengeTrigger);

        // ACT

        var dialogueSid = dialogueEvent.GetDialogSid();

        // ASSERT

        dialogueSid.Should().Be("synth_as_parent_stage_2_fast");
    }

    [Test]
    public void Available_with_initial_story_state()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());

        var requirementContext = Mock.Of<IDialogueEventRequirementContext>(x =>
            x.CurrentLocation == LocationSids.Desert &&
            x.ActiveHeroesInParty == new[] { UnitName.Swordsman, UnitName.Partisan } &&
            x.ActiveStories == Array.Empty<string>());

        // ACT

        var isAvailable = dialogueEvent.GetRequirements().All(r => r.IsApplicableFor(requirementContext));

        // ASSERT

        isAvailable.Should().BeTrue();
    }

    [Test]
    public void Not_available_when_other_side_quest_was_started()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());

        var requirementContext = Mock.Of<IDialogueEventRequirementContext>(x =>
            x.CurrentLocation == LocationSids.Desert &&
            x.ActiveHeroesInParty == new[] { UnitName.Swordsman, UnitName.Partisan } &&
            x.ActiveStories == new[] { "test" });

        // ACT

        var isAvailable = dialogueEvent.GetRequirements().All(r => r.IsApplicableFor(requirementContext));

        // ASSERT

        isAvailable.Should().BeFalse();
    }

    [Test]
    public void Available_when_stage_1_canan_complete()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());
        dialogueEvent.Trigger(DialogueConstants.SideQuests.SynthAsParent.Stage1_Repair_Trigger);
        dialogueEvent.Trigger(DialogueConstants.CompleteCurrentStageChallengeTrigger);

        var requirementContext = Mock.Of<IDialogueEventRequirementContext>(x =>
            x.CurrentLocation == LocationSids.Desert &&
            x.ActiveHeroesInParty == new[] { UnitName.Swordsman, UnitName.Partisan } &&
            x.ActiveStories == Array.Empty<string>());

        // ACT

        var dialogueEventRequirements = dialogueEvent.GetRequirements();
        var isAvailable = dialogueEventRequirements.All(r => r.IsApplicableFor(requirementContext));

        // ASSERT

        isAvailable.Should().BeTrue();
    }

    [Test]
    public void No_available_when_was_started()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());
        dialogueEvent.Trigger(DialogueConstants.SideQuests.SynthAsParent.Stage1_Repair_Trigger);

        var requirementContext = Mock.Of<IDialogueEventRequirementContext>(x =>
            x.CurrentLocation == LocationSids.Desert &&
            x.ActiveHeroesInParty == new[] { UnitName.Swordsman, UnitName.Partisan } &&
            x.ActiveStories == Array.Empty<string>());

        // ACT

        var isAvailable = dialogueEvent.GetRequirements().All(r => r.IsApplicableFor(requirementContext));

        // ASSERT

        isAvailable.Should().BeFalse();
    }
}