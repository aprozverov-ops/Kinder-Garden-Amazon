using Kuhpik;
using StateMachine;
using UnityEngine;

public class ChildSleepState : State
{
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly ChildSpriteRotator m_childSpriteRotator;
    private readonly ChildAnimationController m_childAnimationController;
    private readonly ChildConfiguration m_childConfiguration;

    public bool IsReadyToLeave => m_childSpriteRotator.IsReadyToLeave;

    public ChildSleepState(ChildSpriteActivator childSpriteActivator, ChildSpriteRotator childSpriteRotator,
        ChildAnimationController childAnimationController, ChildConfiguration childConfiguration)
    {
        m_childSpriteActivator = childSpriteActivator;
        m_childSpriteRotator = childSpriteRotator;
        m_childAnimationController = childAnimationController;
        m_childConfiguration = childConfiguration;
    }

    public override void OnStateEnter()
    {
        m_childSpriteActivator.ActivateSprite(ChildStateType.Sleep);
        m_childSpriteRotator.Activate(
            Bootstrap.Instance.GetSystem<CharacterControllerSystem>().CharacterConfigurations
                .GenerationSpeed(m_childConfiguration.MINSleepTime, m_childConfiguration.MAXSleepTime));
        m_childAnimationController.SetBool(ChildAnimationType.Sleep, true);
    }

    public override void OnStateExit()
    {
        m_childAnimationController.SetBool(ChildAnimationType.Sleep, false);
        m_childSpriteActivator.DisableImage();
        m_childSpriteRotator.Disable();
    }
}

public class ChildWantSleepState : State
{
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly ChildAnimationController m_childAnimationController;
    private readonly ChildStackActivator m_childStackActivator;
    private readonly Child m_child;

    public ChildWantSleepState(ChildSpriteActivator childSpriteActivator,
        ChildAnimationController childAnimationController, ChildStackActivator childStackActivator, Child child)
    {
        m_childSpriteActivator = childSpriteActivator;
        m_childAnimationController = childAnimationController;
        m_childStackActivator = childStackActivator;
        m_child = child;
    }

    public override void OnStateEnter()
    {
        m_child.CurrentChildType = ChildStateType.Sleep;
        m_childStackActivator.ActivateStackable(true);
        if (m_child.IsSecondChild == false)
        {
            m_childSpriteActivator.ActivateSprite(ChildStateType.Sleep);
        }
        else
        {
            m_childSpriteActivator.ActivateSprite(ChildStateType.Bed);
        }
        m_child.FakeCurrentChildType = ChildStateType.Sleep;
        m_childAnimationController.SetBool(ChildAnimationType.WandToSleep, true);
    }

    public override void OnStateExit()
    {
        m_childAnimationController.SetBool(ChildAnimationType.WandToSleep, false);
        m_childSpriteActivator.DisableImage();
        m_child.FakeCurrentChildType = ChildStateType.None;
        m_child.CurrentChildType = ChildStateType.None;
        m_childStackActivator.ActivateStackable(false);
    }
}