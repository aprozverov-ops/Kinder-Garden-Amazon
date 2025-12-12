using System.Linq;
using StateMachine;
using UnityEngine;

public class HelperSearherTarget : State
{
    private readonly Helper m_helper;
    private readonly HelperStack m_helperStack;

    public bool IsReadyToLeave;

    public HelperSearherTarget(Helper helper, HelperStack helperStack)
    {
        m_helper = helper;
        m_helperStack = helperStack;
    }

    public override void OnStateEnter()
    {
        if (m_helperStack.IsCanBeDetachProp(PropType.Bottle))
        {
            var foundChild = Physics.OverlapSphere(m_helper.SearchTarget.position, m_helper.SearchRadius).Where(t =>
            {
                var child = t.GetComponent<Child>();
                return child && child.FakeCurrentChildType == ChildStateType.Eat && (child.ChildOnEatPlace);
            }).ToList();
            if (foundChild.Count != 0)
                m_helper.InitializeEatState(foundChild[0].GetComponent<Child>());
        }

        if (m_helperStack.IsCanBeDetachProp(PropType.DiaperProp))
        {
            var foundChild = Physics.OverlapSphere(m_helper.SearchTarget.position, m_helper.SearchRadius).Where(t =>
            {
                var child = t.GetComponent<Child>();
                return child && child.CurrentChildType == ChildStateType.Poop;
            }).ToList();
            if (foundChild.Count != 0)
                m_helper.InitializePoopState(foundChild[0].GetComponent<Child>());
        }

        if (m_helperStack.IsCanBeDetachProp(PropType.Kasha))
        {
            var foundChild = Physics.OverlapSphere(m_helper.SearchTarget.position, m_helper.SearchRadius).Where(t =>
            {
                var child = t.GetComponent<Child>();
                return child && child.FakeCurrentChildType == ChildStateType.Eat && child.ChildOnEatPlace;
            }).ToList();
            if (foundChild.Count != 0)
                m_helper.InitializeEatState(foundChild[0].GetComponent<Child>());
        }

        IsReadyToLeave = true;
    }

    public override void OnStateExit()
    {
        IsReadyToLeave = false;
    }
}