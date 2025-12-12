using StateMachine;

public class ChildSwapPlayGameState : State
{
    private readonly Child m_child;

    public ChildSwapPlayGameState(Child child)
    {
        m_child = child;
    }

    public override void OnStateEnter()
    {
        m_child.ReinitializePlay();
    }
}