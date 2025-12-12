namespace StateMachine
{
    public interface IStateMachine
    {
        State CurrentState { get; }
        void SetState(State state);
        void Tick();
        void FixedTick();
    }
}