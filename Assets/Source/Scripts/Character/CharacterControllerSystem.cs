using System;
using System.Collections;
using DG.Tweening;
using Kuhpik;
using StateMachine;
using StateMachine.Conditions;
using UnityEngine;

public class CharacterControllerSystem : GameSystem
{
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private CharacterConfigurations characterConfigurations;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Joystick joystick;

    private CharacterAnimationController m_animationController;
    private StateMachine.StateMachine m_stateMachine;
    private ChangeChildDiaperState m_childDiaperState;

    public CharacterConfigurations CharacterConfigurations => characterConfigurations;

    private void Awake()
    {
        m_animationController = new CharacterAnimationController(characterAnimator);
    }

    public override void OnInit()
    {
        base.OnInit();
        game.CharacterAnimationController = m_animationController;
        InitializeStateMachine();
    }

    private void FixedUpdate()
    {
        m_stateMachine.FixedTick();
    }

    private void Update()
    {
        m_stateMachine.Tick();
    }

    public void TryToChangeChildDiaper(Child child, Stack stack)
    {
        m_childDiaperState.SetChild(child, stack);
        m_stateMachine.SetState(m_childDiaperState);
    }

    private void InitializeStateMachine()
    {
        var moveState = new CharacterMoveState(rigidbody, characterConfigurations, joystick, m_animationController);
        var idleState = new CharacterIdleState(m_animationController);
        m_childDiaperState = new ChangeChildDiaperState(m_animationController, rigidbody.transform);

        //idleState transitions
        idleState.AddTransition(new StateTransition(moveState,
            new FuncCondition(() => joystick.Direction != Vector2.zero)));

        //moveState transitions
        moveState.AddTransition(new StateTransition(idleState,
            new FuncCondition(() => joystick.Direction == Vector2.zero)));

        m_childDiaperState.AddTransition(new StateTransition(idleState, new FuncCondition(() => m_childDiaperState
            .IsReadyToLeave)));

        m_stateMachine = new StateMachine.StateMachine(idleState);
    }
}

public class ChangeChildDiaperState : State
{
    private readonly CharacterAnimationController m_characterAnimationController;
    private readonly Transform m_character;
    private Child m_currentChild;
    private Stack m_stack;

    public void SetChild(Child child, Stack stack)
    {
        m_stack = stack;
        m_currentChild = child;
        child.TryToPoop();
    }

    public bool IsReadyToLeave => m_currentChild.IsPoopFinish;

    public ChangeChildDiaperState(CharacterAnimationController characterAnimationController, Transform character)
    {
        m_characterAnimationController = characterAnimationController;
        m_character = character;
    }

    public override void OnStateEnter()
    {
        VibrationSystem.Play();
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.ChangePoop);
        m_character.DOLookAt(m_currentChild.transform.position, 0.4f, AxisConstraint.Y);
        m_characterAnimationController.SetBool(CharacterAnimationType.ChildDiaper, true);
    }

    public override void OnStateExit()
    {
        VibrationSystem.Play();
        if (Bootstrap.Instance.PlayerData.IsTutorialFinish)
            Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Walk);
        m_stack.EnableStack();
        m_characterAnimationController.SetBool(CharacterAnimationType.ChildDiaper, false);
    }
}