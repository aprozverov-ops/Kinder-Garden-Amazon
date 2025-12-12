using StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class HelperMoveToTarget : State
{
    private readonly HelperAnimatorController m_helperAnimatorController;
    private readonly NavMeshAgent m_navMeshAgent;

    private Transform target;

    public bool IsOnPosition =>
        target != null && Vector3.Distance(target.position, m_navMeshAgent.transform.position) < 2;

    public HelperMoveToTarget(HelperAnimatorController helperAnimatorController, NavMeshAgent navMeshAgent)
    {
        m_helperAnimatorController = helperAnimatorController;
        m_navMeshAgent = navMeshAgent;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }


    public override void OnStateEnter()
    {
        m_helperAnimatorController.SetBool(CharacterAnimationType.Walk, true);
        m_navMeshAgent.enabled = true;
        m_navMeshAgent.speed = 3f;
    }

    public override void OnStateExit()
    {
        m_helperAnimatorController.SetBool(CharacterAnimationType.Walk, false);
        m_navMeshAgent.enabled = false;
        m_navMeshAgent.speed = 0;
    }

    public override void Tick()
    {
        m_navMeshAgent.SetDestination(target.position);
    }
}