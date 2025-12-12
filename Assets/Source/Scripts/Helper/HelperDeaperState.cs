using DG.Tweening;
using StateMachine;
using UnityEngine;

public class HelperDeaperState : State
{
    private readonly HelperAnimatorController m_characterAnimationController;
    private readonly Transform m_character;
    private readonly HelperStack m_helperStack;
    private Child m_currentChild;

    public void SetChild(Child child)
    {
        m_currentChild = child;
    }

    public bool IsChildReady => m_currentChild.CurrentChildType != ChildStateType.Poop;
    public bool IsReadyToLeave => m_currentChild.IsPoopFinish;

    public HelperDeaperState(HelperAnimatorController characterAnimationController, Transform character,HelperStack helperStack)
    {
        m_characterAnimationController = characterAnimationController;
        m_character = character;
        m_helperStack = helperStack;
    }

    public override void OnStateEnter()
    {
        var prop = m_helperStack.DetachProp(PropType.DiaperProp);
        GameObject.Destroy(prop.gameObject);
        m_currentChild.TryToPoop();
        m_character.DOLookAt(m_currentChild.transform.position, 0.4f, AxisConstraint.Y);
        m_characterAnimationController.SetBool(CharacterAnimationType.ChildDiaper, true);
    }

    public override void OnStateExit()
    {
        m_characterAnimationController.SetBool(CharacterAnimationType.ChildDiaper, false);
    }
}