namespace RKode.Utils {
public interface IState {
    void OnEnter();
    void OnTick();
    void OnExit();
}
}