using UnityEngine;

namespace RKode.Utils {
public class StateMachine<T> where T : IState {
    public T CurrentState { get; private set; }

    public void ChangeState(T newState, bool forceChangeState = false) {
        if (!forceChangeState && CurrentState != null && 
            CurrentState.GetType() == newState.GetType()) {
            // EqualityComparer<T>.Default.Equals(CurrentState, newState)) 
            Debug.Log($"[StateMachine] Blocked same state: {newState.GetType().Name} (returning)...");
            return;
        }

        Debug.Log($"[StateMachine] State Changed ({CurrentState?.GetType().Name} → {newState.GetType().Name})");
        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState?.OnEnter();
    }

    public void Tick() {
        CurrentState?.OnTick();
    }
}
}