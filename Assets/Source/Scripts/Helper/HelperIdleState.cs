using StateMachine;

public class HelperIdleState : State
{
    private readonly HelperAnimatorController m_helperAnimatorController;

    public HelperIdleState(HelperAnimatorController helperAnimatorController)
    {
        m_helperAnimatorController = helperAnimatorController;
    }

    public override void OnStateEnter()
    {
        m_helperAnimatorController.SetBool(CharacterAnimationType.Idle, true);
    }

    public override void OnStateExit()
    {
        m_helperAnimatorController.SetBool(CharacterAnimationType.Idle, false);
    }
}