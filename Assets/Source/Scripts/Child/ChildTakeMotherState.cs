using Kuhpik;
using StateMachine;

public class ChildTakeMotherState : State
{
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly ChildStackActivator m_childStackActivator;
    private readonly Child m_child;
    private readonly bool m_isSecond;
    private MapArrowPointer m_arrowPointer;

    public ChildTakeMotherState(ChildSpriteActivator childSpriteActivator, ChildStackActivator childStackActivator,
        Child child, bool isSecond)
    {
        m_childSpriteActivator = childSpriteActivator;
        m_childStackActivator = childStackActivator;
        m_child = child;
        m_isSecond = isSecond;
    }

    public override void OnStateEnter()
    {
        if (m_isSecond)
        {
            Bootstrap.Instance.GetSystem<MotherSpawner>().SecondChildStage.SpawnMotherToTakeChild();
        }
        else
        {
            Bootstrap.Instance.GetSystem<MotherSpawner>().FirstChildStage.SpawnMotherToTakeChild();
        }
        if (Bootstrap.Instance.PlayerData.IsTutorialFinish)
        m_arrowPointer = Bootstrap.Instance.GetSystem<PointerActivator>()
            .SetPointer(PointerType.Family, m_child.transform,m_child.IsSecondChild);

        m_child.CurrentChildType = ChildStateType.ToMother;
        m_childSpriteActivator.ActivateSprite(ChildStateType.ToMother);
        m_child.FakeCurrentChildType = ChildStateType.ToMother;
        m_childStackActivator.ActivateStackable(true);
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

        m_child.CurrentChildType = ChildStateType.None;
        m_child.FakeCurrentChildType = ChildStateType.None;
        m_childSpriteActivator.DisableImage();
        m_childStackActivator.ActivateStackable(false);
    }
}