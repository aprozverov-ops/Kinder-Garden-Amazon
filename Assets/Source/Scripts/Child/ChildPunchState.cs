using StateMachine;

public class ChildPunchState : State
{
    private readonly ChildAnimationController m_childAnimationController;
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly ChildStateType m_childStateType;
    private readonly ChildStackActivator m_childStackActivator;
    private readonly bool m_isActivateStack;
    private readonly Child m_child;

    public ChildPunchState(ChildAnimationController childAnimationController, ChildSpriteActivator childSpriteActivator,
        ChildStateType childStateType, ChildStackActivator childStackActivator,Child child,bool isActivateStack = false)
    {
        m_childAnimationController = childAnimationController;
        m_childSpriteActivator = childSpriteActivator;
        m_childStateType = childStateType;
        m_childStackActivator = childStackActivator;
        m_isActivateStack = isActivateStack;
        m_child = child;
    }

    public override void OnStateEnter()
    {
        m_child.CurrentChildType = m_childStateType;
        m_childAnimationController.SetBool(ChildAnimationType.Punch, true);
        m_childSpriteActivator.ActivateSprite(m_childStateType);
        m_childStackActivator.ActivateStackable(m_isActivateStack);
    }

    public override void OnStateExit()
    {
        m_child.CurrentChildType = ChildStateType.None;
        m_childAnimationController.SetBool(ChildAnimationType.Punch, false);
        m_childSpriteActivator.DisableImage();
        m_childStackActivator.ActivateStackable(false);
    }
}