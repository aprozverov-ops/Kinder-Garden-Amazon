using System.Collections;
using Kuhpik;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class ChildEscapeState : State
{
    private readonly NavMeshAgent m_navMeshAgent;
    private readonly ChildAnimationController m_childAnimationController;
    private readonly ChildStateType m_childStateType;
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly ChildStackActivator m_childStackActivator;
    private readonly bool m_isActivateStack;
    private readonly bool m_spawnProp;
    private readonly PropType m_propType;
    private readonly Child m_child;
    private readonly ChildEffectActivator m_childEffectActivator;
    private readonly bool m_isSecond;
    private readonly ChildEffectType m_childEffectType;
    private readonly ChildEscapePositions m_childEscapePositions;

    private Transform currentRunPos;
    private PropSpawner m_propSpawner;
    private bool isSpawnProp;
    private MapArrowPointer m_arrowPointer;
    public bool IsOnPosition => Vector3.Distance(m_navMeshAgent.transform.position, currentRunPos.position) < 2;

    public ChildEscapeState(NavMeshAgent navMeshAgent, ChildAnimationController childAnimationController,
        ChildStateType childStateType, ChildSpriteActivator childSpriteActivator,
        ChildStackActivator childStackActivator, Child child, ChildEffectActivator childEffectActivator, bool isSecond,
        ChildEffectType childEffectType = ChildEffectType.Poop, bool isActivateStack = false, bool spawnProp = false,
        PropType propType = PropType.DiaperProp)
    {
        if (spawnProp)
        {
            m_propSpawner = Bootstrap.Instance.GetSystem<PropSpawner>();
        }

        m_navMeshAgent = navMeshAgent;
        m_childEscapePositions = Bootstrap.Instance.GetSystem<ChildEscapePositions>();
        m_childAnimationController = childAnimationController;
        m_childStateType = childStateType;
        m_childSpriteActivator = childSpriteActivator;
        m_childStackActivator = childStackActivator;
        m_isActivateStack = isActivateStack;
        m_spawnProp = spawnProp;
        m_propType = propType;
        m_child = child;
        m_childEffectActivator = childEffectActivator;
        m_isSecond = isSecond;
        m_childEffectType = childEffectType;
    }

    public override void OnStateEnter()
    {
        if (isSpawnProp == false && m_spawnProp)
        {
            isSpawnProp = true;
        }

        m_arrowPointer = Bootstrap.Instance.GetSystem<PointerActivator>()
            .SetPointer(PointerType.Poop, m_child.transform,m_child.IsSecondChild);
        m_childEffectActivator.ActivateEffect(m_childEffectType, true);
        m_childStackActivator.ActivateStackable(m_isActivateStack);
        m_childSpriteActivator.ActivateSprite(m_childStateType);
        m_navMeshAgent.enabled = true;
        m_child.CurrentChildType = m_childStateType;
        m_navMeshAgent.speed = 1;
        m_childAnimationController.SetBool(ChildAnimationType.Walk, true);
        currentRunPos = m_childEscapePositions.GetEscapePosition(m_isSecond);
    }

    public override void OnStateExit()
    {
        if (m_arrowPointer != null)
        {
            m_arrowPointer.SetTarget(null, null);
        }

        m_childEffectActivator.ActivateEffect(m_childEffectType, false);
        m_child.CurrentChildType = ChildStateType.None;
        m_childStackActivator.ActivateStackable(false);
        m_childSpriteActivator.DisableImage();
        m_navMeshAgent.enabled = false;
        m_navMeshAgent.speed = 0;
        m_childAnimationController.SetBool(ChildAnimationType.Walk, false);
    }

    public override void Tick()
    {
        m_navMeshAgent.SetDestination(
            currentRunPos.position);
        if (m_child.StackableItem.IsAttach)
        {
            if (m_arrowPointer != null)
            {
                m_arrowPointer.SetTarget(null, null);
            }

            m_arrowPointer = null;
        }
    }
}