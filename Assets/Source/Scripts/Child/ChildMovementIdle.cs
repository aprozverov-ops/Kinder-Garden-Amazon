using StateMachine;

public class ChildMovementIdle : State
{
    private readonly ChildAnimationController m_childAnimationController;

    public ChildMovementIdle(ChildAnimationController childAnimationController)
    {
        m_childAnimationController = childAnimationController;
    }

    public override void OnStateExit()
    {
        m_childAnimationController.SetBool(ChildAnimationType.Idle, false);
    }

    public override void OnStateEnter()
    {
        m_childAnimationController.SetBool(ChildAnimationType.Idle, true);
    }
}