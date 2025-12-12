using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using StateMachine;
using StateMachine.Conditions;
using UnityEngine;
using UnityEngine.AI;

public class Child : MonoPooled
{
    [SerializeField] private bool isSecondChild;
    [SerializeField] private ChildConfiguration childConfiguration;
    [SerializeField] private ChildEffectActivator childEffectActivator;
    [SerializeField] private ChildSpriteActivator childSpriteActivator;
    [SerializeField] private ChildSpriteRotator childSpriteRotator;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private StackableItem stackableItem;

    private TemporaryAndFuncCondition swapGameTransition;
    private TemporaryCondition poopTransition;
    private TemporaryCondition afterPoopTransition;
    private ChildWantSleepPlace m_childWantSleepPlace;

    private EatPlace currentEatPlace;
    private StateMachine.StateMachine m_stateMachine;
    private ChildAnimationController m_animationController;
    private ChildStackActivator m_stackActivator;

    //childPlayGroup
    private ChildPlayState m_childPlayState;
    private ChildMovementState m_childPlayMovementState;
    private ChildSwapPlayGameState m_childSwapPlayGameState;

    //childPoopStateGroup
    private ChildWandToPoop m_childWandToPoop;

    private ChildPoopState m_poopState;

    //childPoopStateEscape
    private ChildMovementIdle m_childMovementIdleafterPoop;
    private ChildEscapeState m_childEscapePoopState;
    private ChildPunchState m_childPunchPoopState;

    //childEatStateGroup
    private ChildWantEatState m_childWantEatState;
    private ChildWantEatPrepareState m_childWantEatPrepareState;
    private ChildEatState m_childEatState;

    public EatPlace CurrentEatPlace
    {
        get => currentEatPlace;
        set => currentEatPlace = value;
    }

    //childSleepStateGroup
    private ChildSleepState m_childSleepState;
    private ChildTakeMotherState m_childTakeMother;
    private ChildWantSleepState m_childWantSleepState;

    public bool ChildOnEatPlace;
    public bool ChildOnMotherPlace;
    public bool ChildOnSleepPlace;

    public StackableItem StackableItem => stackableItem;

    public ChildStateType CurrentChildType = ChildStateType.None;
    public ChildStateType FakeCurrentChildType = ChildStateType.None;
    public ChildAnimationController ChildAnimationController => m_animationController;

    public bool IsPoopFinish =>
        m_stateMachine.CurrentState != m_poopState && m_stateMachine.CurrentState != m_childWandToPoop;

    public bool IsSecondChild => isSecondChild;


    private void Awake()
    {
        m_animationController = new ChildAnimationController(animator);
        m_stackActivator = new ChildStackActivator(stackableItem);
    }

    private void Update()
    {
        m_stateMachine?.Tick();
        if (stackableItem.IsAttach && currentEatPlace != null)
        {
            currentEatPlace.DisableChild();
            currentEatPlace = null;
        }

        if (stackableItem.IsAttach && m_childWantSleepPlace != null)
        {
            m_childWantSleepPlace.DisableChild();
            m_childWantSleepPlace = null;
        }
    }

    public void WantToPoop()
    {
        Initialize();
        m_stateMachine.SetState(m_childWandToPoop);
    }

    public void Initialize()
    {
        if (isSecondChild == false)
        {
            Bootstrap.Instance.GameData.amountChildFirstFake++;
        }
        else
        {
            Bootstrap.Instance.GameData.amountChildSecondFake++;
        }

        ChildOnEatPlace = false;

        ChildOnSleepPlace = false;
        ChildOnMotherPlace = false;
        navMeshAgent.enabled = false;
        InitializeStateMachine();
        InitializePlay();
    }

    private void InitializePlay()
    {
        var playConfiguration =
            Bootstrap.Instance.GetSystem<ChildPositionController>().GetRandomPlayPosition(isSecondChild);

        playConfiguration.IsActivate = false;
        m_childPlayState.SetPlayGame(playConfiguration);
        m_childPlayMovementState.SetTarget(playConfiguration.Position);
        swapGameTransition.UpdateTime(m_childPlayState.PlayTime);
        poopTransition.UpdateTime(m_childPlayState.PlayTime);
    }

    public void ReinitializePlay()
    {
        var playConfiguration =
            Bootstrap.Instance.GetSystem<ChildPositionController>().GetRandomPlayPosition(isSecondChild);

        playConfiguration.IsActivate = false;
        m_childPlayState.SetPlayGame(playConfiguration);
        m_childPlayMovementState.SetTarget(playConfiguration.Position);
        m_stateMachine.SetState(m_childPlayMovementState);
        swapGameTransition.UpdateTime(m_childPlayState.PlayTime);
        poopTransition.UpdateTime(m_childPlayState.PlayTime);
    }

    public void TryToPoop()
    {
        m_stateMachine
            .SetState(m_poopState);
    }

    public void OnDrop()
    {
        navMeshAgent.enabled = true;
        StartCoroutine(ResetNavMesh());
    }

    private IEnumerator ResetNavMesh()
    {
        yield return new WaitForSeconds(0.4f);
        navMeshAgent.enabled = false;
    }

    public void ToEnd()
    {
        var newIdleState = new ChildMovementIdle(m_animationController);
        m_stateMachine.SetState(newIdleState);
    }

    public void TryToEat(bool isPrepare, EatPlace eatPlace)
    {
        currentEatPlace = eatPlace;
        if (isPrepare)
        {
            m_stateMachine.SetState(m_childWantEatPrepareState);
        }

        else
        {
            m_stateMachine.SetState(m_childEatState);
        }
    }

    public void TryToEat()
    {
        m_stateMachine.SetState(m_childEatState);
    }

    public void TryToSleep(ChildWantSleepPlace childWantSleepPlace)
    {
        m_childWantSleepPlace = childWantSleepPlace;
        m_stateMachine.SetState(m_childSleepState);
    }

    private void InitializeStateMachine()
    {
        var idleState = new ChildMovementIdle(m_animationController);

//sleep state initialize
        m_childSleepState = new ChildSleepState(childSpriteActivator, childSpriteRotator, m_animationController,
            childConfiguration);

        m_childTakeMother = new ChildTakeMotherState(childSpriteActivator, m_stackActivator, this, isSecondChild);
        m_childWantSleepState =
            new ChildWantSleepState(childSpriteActivator, ChildAnimationController, m_stackActivator, this);

//eat state initialize
        m_childEatState = new ChildEatState(childConfiguration, childSpriteRotator, childSpriteActivator,
            m_animationController);

        m_childWantEatState =
            new ChildWantEatState(m_stackActivator, childSpriteActivator, this, m_animationController);
        m_childWantEatPrepareState =
            new ChildWantEatPrepareState(childSpriteActivator, this, m_animationController, isSecondChild);

//poop escape initialize
        m_childEscapePoopState = new ChildEscapeState(navMeshAgent, m_animationController, ChildStateType.Poop,
            childSpriteActivator, m_stackActivator, this, childEffectActivator, isSecondChild, spawnProp: true,
            propType: PropType.DiaperProp);

        m_childPunchPoopState = new ChildPunchState(m_animationController, childSpriteActivator, ChildStateType.Poop,
            m_stackActivator, this);

        m_childMovementIdleafterPoop = new ChildMovementIdle(m_animationController);

//poop state initialize
        m_childWandToPoop = new ChildWandToPoop(m_stackActivator, childSpriteActivator, this, childEffectActivator,
            m_animationController, isSecondChild);

        m_poopState = new ChildPoopState(childConfiguration, childSpriteRotator, childSpriteActivator,
            childEffectActivator, m_animationController, this);

//play state initialize
        m_childPlayMovementState = new ChildMovementState(navMeshAgent, m_animationController);
        m_childPlayState = new ChildPlayState(m_animationController);
        m_childSwapPlayGameState = new ChildSwapPlayGameState(this);
        idleState.AddTransition(new StateTransition(m_childPlayMovementState,
            new TemporaryCondition(Random.Range(0f, 0f))));
        m_childPlayMovementState.AddTransition(new StateTransition(m_childPlayState, new FuncCondition(() =>
            m_childPlayMovementState.IsOnPosition)));

        swapGameTransition = new TemporaryAndFuncCondition(() => m_childPlayState.IsSwap, m_childPlayState.PlayTime);
        m_childPlayState.AddTransition(new StateTransition(m_childSwapPlayGameState, swapGameTransition));
        poopTransition = new TemporaryCondition(m_childPlayState.PlayTime);
        var childPlayStateTransition = new RandomStateTransition(m_childWandToPoop, poopTransition);
        childPlayStateTransition.AddNewState(m_childEscapePoopState);
        m_childPlayState.AddTransition(childPlayStateTransition);
        m_poopState.AddTransition(new StateTransition(m_childMovementIdleafterPoop,
            new FuncCondition(() =>
            {
                if (!m_poopState.IsReadyToLeave) return false;
                return true;
            })));
        afterPoopTransition = new TemporaryCondition(Random.Range(0f, 1f));
        m_childMovementIdleafterPoop.AddTransition(new StateTransition(m_childWantEatState, afterPoopTransition));
        if (isSecondChild == false)
        {
            m_childEatState.AddTransition(new StateTransition(m_childSleepState,
                new FuncCondition(() => m_childEatState.IsReadyToLeave)));
        }
        else
        {
            m_childEatState.AddTransition(new StateTransition(m_childWantSleepState,
                new FuncCondition(() => m_childEatState.IsReadyToLeave)));
        }

        m_childSleepState.AddTransition(new StateTransition(m_childTakeMother,
            new FuncCondition(() => m_childSleepState.IsReadyToLeave)));
        m_stateMachine = new StateMachine.StateMachine(idleState);
        m_childEscapePoopState.AddTransition(new StateTransition(m_childPunchPoopState,
            new FuncCondition(() => m_childEscapePoopState.IsOnPosition)));
        m_childEscapePoopState.AddTransition(new StateTransition(m_childWandToPoop,
            new FuncCondition(() => stackableItem.IsAttach)));
        m_childPunchPoopState.AddTransition(new StateTransition(m_childEscapePoopState, new TemporaryCondition(2f)));
        m_childPunchPoopState.AddTransition(new StateTransition(m_childWandToPoop,
            new FuncCondition(() => stackableItem.IsAttach)));
    }
}