public abstract class PlayerState {
    public abstract void EnterState(PlayerStateManager stateManager, PlayerBase player);
    public abstract void ExitState(PlayerStateManager stateManager, PlayerBase player);
    public abstract void UpdateState(PlayerStateManager stateManager, PlayerBase player);
}