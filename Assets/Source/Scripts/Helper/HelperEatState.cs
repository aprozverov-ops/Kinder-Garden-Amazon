using DG.Tweening;
using StateMachine;
using UnityEngine;

public class HelperEatState : State
{
    private readonly HelperStack m_helperStack;
    private Child m_currentChild;

    public bool IsReadyToLeave;

    public bool IsChildReady => m_currentChild.FakeCurrentChildType != ChildStateType.Eat;

    public HelperEatState(HelperStack helperStack)
    {
        m_helperStack = helperStack;
    }

    public void SetChild(Child child)
    {
        m_currentChild = child;
    }

    public override void OnStateEnter()
    {
        m_currentChild.TryToEat();
        var bottle = m_helperStack.DetachProp(PropType.Bottle);
        if (bottle == null)
            bottle = m_helperStack.DetachProp(PropType.Kasha);
        bottle.transform.parent = null;
        bottle.transform.DOJump(m_currentChild.transform.position, 1, 1, 0.3f)
            .OnComplete(() => GameObject.Destroy(bottle.gameObject));
        IsReadyToLeave = true;
    }

    public override void OnStateExit()
    {
        IsReadyToLeave = false;
    }
}