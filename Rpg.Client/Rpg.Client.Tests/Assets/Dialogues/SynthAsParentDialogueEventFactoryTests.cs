using System;
using System.Linq;

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
    public void Check_dialogues_in_different_states_Initial()
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
    public void Check_dialogues_in_different_states_Ignore_stage1()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());
        dialogueEvent.Trigger(new DialogueEventTrigger("stage_1_fast"));

        // ACT

        var dialogueSid = dialogueEvent.GetDialogSid();

        // ASSERT

        dialogueSid.Should().Be("synth_as_parent_stage_2_fast");
    }
    
    [Test]
    public void Check_dialogues_in_different_states_Help_stage1()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());
        dialogueEvent.Trigger(new DialogueEventTrigger("stage_1_help"));

        // ACT

        var dialogueSid = dialogueEvent.GetDialogSid();

        // ASSERT

        dialogueSid.Should().Be("synth_as_parent_stage_2");
    }
    
    [Test]
    public void Check_dialogues_in_different_states_Help_stage2_complete()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());
        dialogueEvent.Trigger(new DialogueEventTrigger("stage_1_help"));
        dialogueEvent.Trigger(new DialogueEventTrigger("stage_2_complete"));

        // ACT

        var dialogueSid = dialogueEvent.GetDialogSid();

        // ASSERT

        dialogueSid.Should().Be("synth_as_parent_stage_3");
    }

    [Test]
    public void Check_Availability_InitStateWithEmptyContext()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());

        var requirementContext = Mock.Of<IDialogueEventRequirementContext>(x =>
        x.CurrentLocation == LocationSid.Desert &&
        x.ActiveHeroesInParty == new[] { UnitName.Swordsman, UnitName.Partisan } &&
        x.ActiveStories == Array.Empty<string>());

        // ACT

        var isAvailable = dialogueEvent.GetRequirements().All(r => r.IsApplicableFor(requirementContext));


        // ASSERT

        isAvailable.Should().BeTrue();
    }

    [Test]
    public void Check_Availability_NotAvailableWhenSomeStoryStarted()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());

        var requirementContext = Mock.Of<IDialogueEventRequirementContext>(x =>
        x.CurrentLocation == LocationSid.Desert &&
        x.ActiveHeroesInParty == new[] { UnitName.Swordsman, UnitName.Partisan } &&
        x.ActiveStories == new[] { "test" });

        // ACT

        var isAvailable = dialogueEvent.GetRequirements().All(r => r.IsApplicableFor(requirementContext));


        // ASSERT

        isAvailable.Should().BeFalse();
    }

    [Test]
    public void Check_Availability_State2WithEmptyContext()
    {
        // ARRANGE

        var factory = new SynthAsParentDialogueEventFactory();

        var dialogueEvent = factory.CreateEvent(Mock.Of<IDialogueEventFactoryServices>());
        dialogueEvent.Trigger(DialogueConsts.SideQuests.SynthAsParent.Stage1_Help_Trigger);
        dialogueEvent.Trigger(DialogueConsts.CompleteStageChallangeTrigger);

        var requirementContext = Mock.Of<IDialogueEventRequirementContext>(x =>
        x.CurrentLocation == LocationSid.Desert &&
        x.ActiveHeroesInParty == new[] { UnitName.Swordsman, UnitName.Partisan } &&
        x.ActiveStories == Array.Empty<string>());

        // ACT

        var dialogueEventRequirements = dialogueEvent.GetRequirements();
        var isAvailable = dialogueEventRequirements.All(r => r.IsApplicableFor(requirementContext));


        // ASSERT

        isAvailable.Should().BeTrue();
    }
}