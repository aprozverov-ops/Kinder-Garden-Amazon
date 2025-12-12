using Kuhpik;
using StateMachine;
using UnityEngine;

public class ChildEatState : State
{
    private readonly ChildConfiguration m_childConfiguration;
    private readonly ChildSpriteRotator m_childSpriteRotator;
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly ChildAnimationController m_childAnimationController;

    public bool IsReadyToLeave => m_childSpriteRotator.IsReadyToLeave;

    public ChildEatState(ChildConfiguration childConfiguration, ChildSpriteRotator childSpriteRotator,
        ChildSpriteActivator childSpriteActivator, ChildAnimationController childAnimationController)
    {
        m_childConfiguration = childConfiguration;
        m_childSpriteRotator = childSpriteRotator;
        m_childSpriteActivator = childSpriteActivator;
        m_childAnimationController = childAnimationController;
    }

    public override void OnStateEnter()
    {
        m_childAnimationController.SetBool(ChildAnimationType.Eat, true);
        m_childSpriteActivator.ActivateSprite(ChildStateType.Eat);
        m_childSpriteRotator.Activate(Bootstrap.Instance.GetSystem<CharacterControllerSystem>().CharacterConfigurations
            .GenerationSpeed(m_childConfiguration.MINEatTime, m_childConfiguration.MAXEatTime));
    }

    public override void OnStateExit()
    {
        m_childAnimationController.SetBool(ChildAnimationType.Eat, false);
        m_childSpriteActivator.DisableImage();
        m_childSpriteRotator.Disable();
    }
}