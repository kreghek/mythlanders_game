using Client.Assets.Dialogues;
using Client.Core.Dialogues;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Tests.Assets.Dialogues;

[TestFixture]
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
}