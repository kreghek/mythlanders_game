using System;
using System.Linq;

using Client.Assets.Catalogs.Crises;
using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Crisis;
using Client.ScreenManagement;

using CombatDicesTeam.Dices;

using Core.Crises;

namespace Client.Assets.StageItems;

internal abstract class SmallTextEventStageItem : ICampaignStageItem
{
    private readonly ICrisesCatalog _crisesCatalog;
    private readonly IDice _dice;
    private readonly IEventCatalog _eventCatalog;

    protected SmallTextEventStageItem(IDice dice, ICrisesCatalog crisesCatalog, IEventCatalog eventCatalog)
    {
        _dice = dice;
        _crisesCatalog = crisesCatalog;
        _eventCatalog = eventCatalog;
    }

    protected abstract EventType EventType { get; }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var availableCrises = _crisesCatalog.GetAll().Where(x => x.EventType == EventType).ToArray();
        if (!availableCrises.Any())
        {
            throw new InvalidOperationException($"There are no available micro-events of type {EventType}.");
        }

        var crisis = _dice.RollFromList(availableCrises);

        var smallEvent = _eventCatalog.Events.First(x => x.Sid == crisis.EventSid);

        var currentDialogueSid = smallEvent.GetDialogSid();
        var crisisDialogue = _eventCatalog.GetDialogue(currentDialogueSid);

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Crisis,
            new CrisisScreenTransitionArguments(currentCampaign, EventType.Crisis, crisisDialogue, smallEvent, crisis));
    }

    public bool IsGoalStage { get; }
}