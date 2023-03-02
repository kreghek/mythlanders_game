using Stateless;

namespace Text.Client;

internal static class StateMachineFactory
{
    public static StateMachine<ClientState, ClientStateTrigger> Create()
    {
        var clientStateMachine = new StateMachine<ClientState, ClientStateTrigger>(ClientState.Initialize);

        clientStateMachine.Configure(ClientState.Initialize)
            .Permit(ClientStateTrigger.OnOverview, ClientState.Overview);

        clientStateMachine.Configure(ClientState.Overview)
            .Permit(ClientStateTrigger.OnDisplayCombatantInfo, ClientState.CombatantInfo);

        clientStateMachine.Configure(ClientState.CombatantInfo)
            .Permit(ClientStateTrigger.OnDisplayMoveInfo, ClientState.MoveInfo)
            .Permit(ClientStateTrigger.OnOverview, ClientState.Overview);

        clientStateMachine.Configure(ClientState.MoveInfo)
            .Permit(ClientStateTrigger.OnDisplayCombatantInfo, ClientState.CombatantInfo)
            .Permit(ClientStateTrigger.OnOverview, ClientState.Overview);

        return clientStateMachine;
    }
}