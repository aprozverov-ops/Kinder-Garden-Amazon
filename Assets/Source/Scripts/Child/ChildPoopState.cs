using Kuhpik;
using StateMachine;
using UnityEngine;

public class ChildPoopState : State
{
    private readonly ChildConfiguration m_childConfiguration;
    private readonly ChildSpriteRotator m_childSpriteRotator;
    private readonly ChildSpriteActivator m_childSpriteActivator;
    private readonly ChildEffectActivator m_childEffectActivator;
    private readonly ChildAnimationController m_childAnimationController;
    private readonly Child m_child;

    public bool IsReadyToLeave => m_childSpriteRotator.IsReadyToLeave;

    public ChildPoopState(ChildConfiguration childConfiguration, ChildSpriteRotator childSpriteRotator,
        ChildSpriteActivator childSpriteActivator, ChildEffectActivator childEffectActivator,
        ChildAnimationController childAnimationController, Child child
    )
    {
        m_childConfiguration = childConfiguration;
        m_childSpriteRotator = childSpriteRotator;
        m_childSpriteActivator = childSpriteActivator;
        m_childEffectActivator = childEffectActivator;
        m_childAnimationController = childAnimationController;
        m_child = child;
    }

    public override void OnStateEnter()
    {
        m_childAnimationController.SetBool(ChildAnimationType.PoopLaugh, true);
        m_childEffectActivator.ActivateEffect(ChildEffectType.Poop, true);
        m_childEffectActivator.ActivateEffect(ChildEffectType.Cry, false);
        m_childSpriteActivator.ActivateSprite(ChildStateType.Poop);
        m_childSpriteRotator.Activate(Bootstrap.Instance.PlayerData.IsTutorialFinish == false
            ? 1.6f
            : Bootstrap.Instance.GetSystem<CharacterControllerSystem>().CharacterConfigurations
                .GenerationSpeed(m_childConfiguration.MINPoopTime, m_childConfiguration.MAXPoopTime));
    }

    public override void OnStateExit()
    {
        m_childAnimationController.SetBool(ChildAnimationType.PoopLaugh, false);
        Bootstrap.Instance.GetSystem<EffectSpawner>().SpawnEffect(EffectType.Poof, m_childEffectActivator.transform);
        m_childEffectActivator.ActivateEffect(ChildEffectType.Poop, false);
        m_childEffectActivator.ActivateEffect(ChildEffectType.Cry, true);
        m_childSpriteActivator.DisableImage();
        m_childSpriteRotator.Disable();
    }
}