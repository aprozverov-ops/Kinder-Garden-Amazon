using Kuhpik;
using StateMachine;

public class ChildWantEatState : State
{
    private readonly ChildStackActivator m_childStackActivator;
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly Child m_child;
    private readonly ChildAnimationController m_childAnimationController;
    private MapArrowPointer m_arrowPointer;

    public ChildWantEatState(ChildStackActivator childStackActivator, ChildSpriteActivator childSpriteActivator,
        Child child, ChildAnimationController childAnimationController)
    {
        m_childStackActivator = childStackActivator;
        m_childSpriteActivator = childSpriteActivator;
        m_child = child;
        m_childAnimationController = childAnimationController;
    }

    public override void OnStateEnter()
    {
        if (Bootstrap.Instance.PlayerData.IsTutorialFinish)
            m_arrowPointer = Bootstrap.Instance.GetSystem<PointerActivator>()
                .SetPointer(m_child.IsSecondChild ? PointerType.Chair : PointerType.Bed, m_child.transform,
                    m_child.IsSecondChild);
        m_child.FakeCurrentChildType = ChildStateType.Eat;
        m_childStackActivator.ActivateStackable(true);
        if (m_child.IsSecondChild)
        {
            m_childSpriteActivator.ActivateSprite(ChildStateType.Chair);
        }
        else
        {
            m_childSpriteActivator.ActivateSprite(ChildStateType.Bed);
        }

        m_child.CurrentChildType = ChildStateType.Eat;
        m_childAnimationController.SetBool(ChildAnimationType.WantEat, true);
    }

    public override void Tick()
    {
        if (m_child.StackableItem.IsAttach)
        {
            if (m_arrowPointer != null)
            {
                m_arrowPointer.SetTarget(null, null);
            }

            m_arrowPointer = null;
        }
    }

    public override void OnStateExit()
    {
        if (m_arrowPointer != null)
        {
            m_arrowPointer.SetTarget(null, null);
        }

        m_child.FakeCurrentChildType = ChildStateType.None;
        m_childStackActivator.ActivateStackable(false);
        m_childSpriteActivator.ActivateSprite(ChildStateType.None);
        m_child.CurrentChildType = ChildStateType.None;
        m_childAnimationController.SetBool(ChildAnimationType.WantEat, false);
    }
}

public class ChildWantEatPrepareState : State
{
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly Child m_child;
    private readonly ChildAnimationController m_childAnimationController;
    private readonly bool m_isSecond;
    private MapArrowPointer m_arrowPointer;

    public ChildWantEatPrepareState(ChildSpriteActivator childSpriteActivator,
        Child child, ChildAnimationController childAnimationController, bool isSecond)
    {
        m_childSpriteActivator = childSpriteActivator;
        m_child = child;
        m_childAnimationController = childAnimationController;
        m_isSecond = isSecond;
    }

    public override void OnStateEnter()
    {
        if (Bootstrap.Instance.PlayerData.IsTutorialFinish == false)
        {
            Bootstrap.Instance.GetSystem<PropSpawner>().SpawnProp(PropType.Bottle);
        }

        if (Bootstrap.Instance.PlayerData.IsTutorialFinish)
            m_arrowPointer = Bootstrap.Instance.GetSystem<PointerActivator>()
                .SetPointer(m_child.IsSecondChild ? PointerType.Food : PointerType.Eat, m_child.transform,
                    m_child.IsSecondChild);

        m_child.FakeCurrentChildType = ChildStateType.Eat;
        m_childSpriteActivator.ActivateSprite(ChildStateType.Eat);
        m_child.CurrentChildType = ChildStateType.Eat;
        m_childAnimationController.SetBool(ChildAnimationType.WantEatPrepare, true);
    }

    public override void Tick()
    {
        if (m_child.StackableItem.IsAttach)
        {
            if (m_arrowPointer != null)
            {
                m_arrowPointer.SetTarget(null, null);
            }

            m_arrowPointer = null;
        }
    }

    public override void OnStateExit()
    {
        if (m_arrowPointer != null)
        {
            m_arrowPointer.SetTarget(null, null);
        }

        m_child.FakeCurrentChildType = ChildStateType.None;
        m_childSpriteActivator.ActivateSprite(ChildStateType.None);
        m_child.CurrentChildType = ChildStateType.None;
        m_childAnimationController.SetBool(ChildAnimationType.WantEatPrepare, false);
    }
}