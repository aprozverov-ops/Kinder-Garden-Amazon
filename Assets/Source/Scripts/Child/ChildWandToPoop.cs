using Kuhpik;
using StateMachine;

public class ChildWandToPoop : State
{
    private readonly ChildStackActivator m_childStackActivator;
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly Child m_child;
    private readonly ChildEffectActivator m_childEffectActivator;
    private readonly ChildAnimationController m_characterAnimationController;
    private readonly bool m_isSecond;

    private MapArrowPointer m_arrowPointer;
    private bool isPoopSpawn = false;

    public ChildWandToPoop(ChildStackActivator childStackActivator, ChildSpriteActivator childSpriteActivator,
        Child child, ChildEffectActivator childEffectActivator, ChildAnimationController characterAnimationController,
        bool isSecond)
    {
        m_childStackActivator = childStackActivator;
        m_childSpriteActivator = childSpriteActivator;
        m_child = child;
        m_childEffectActivator = childEffectActivator;
        m_characterAnimationController = characterAnimationController;
        m_isSecond = isSecond;
    }

    public override void OnStateEnter()
    {
        if (isPoopSpawn == false)
        {
            if (Bootstrap.Instance.PlayerData.IsTutorialFinish == false)
            {
                Bootstrap.Instance.GetSystem<PropSpawner>().SpawnProp(PropType.DiaperProp);
            }

            isPoopSpawn = true;
        }

        if (Bootstrap.Instance.PlayerData.IsTutorialFinish)
            m_arrowPointer = Bootstrap.Instance.GetSystem<PointerActivator>()
                .SetPointer(PointerType.Poop, m_child.transform,m_child.IsSecondChild);

        m_childEffectActivator.ActivateEffect(ChildEffectType.Poop, true);
        m_childSpriteActivator.ActivateSprite(ChildStateType.Poop);
        m_child.CurrentChildType = ChildStateType.Poop;
        m_characterAnimationController.SetBool(ChildAnimationType.Poop, true);
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

        m_childEffectActivator.ActivateEffect(ChildEffectType.Poop, false);
        m_childSpriteActivator.ActivateSprite(ChildStateType.None);
        m_child.CurrentChildType = ChildStateType.None;
        m_characterAnimationController.SetBool(ChildAnimationType.Poop, false);
    }
}