using StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class ChildMovementState : State
{
    private readonly NavMeshAgent m_navMeshAgent;
    private readonly ChildAnimationController m_controller;

    private Transform m_target;

    public bool IsOnPosition => Vector3.Distance(m_navMeshAgent.transform.position, m_target.position) < 1;

    public ChildMovementState(NavMeshAgent navMeshAgent, ChildAnimationController controller)
    {
        m_navMeshAgent = navMeshAgent;
        m_controller = controller;
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
    }

    public override void OnStateEnter()
    {
        m_navMeshAgent.speed = 1;
        m_navMeshAgent.enabled = true;
        m_controller.SetBool(ChildAnimationType.Walk, true);
    }

    public override void OnStateExit()
    {
        m_navMeshAgent.enabled = false;
        m_navMeshAgent.speed = 0;
        m_controller.SetBool(ChildAnimationType.Walk, false);
    }

    public override void Tick()
    {
        m_navMeshAgent.SetDestination(m_target.position);
    }
}