using System;
using System.Collections;
using System.Linq;
using StateMachine;
using StateMachine.Conditions;
using UnityEngine;
using UnityEngine.AI;

public class Helper : MonoBehaviour
{
    [SerializeField] private bool isSecondHelper;
    [SerializeField] private HelperStack helperStack;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform searchTarget;
    [SerializeField] private float searchRadius;

    public string CurrentState;
    private HelperAnimatorController m_helperAnimatorController;

    //searchPropStates
    private HelperIdleState m_helperIdleState;
    private HelperMoveToTarget m_moveToPropState;
    private HelperStackControllerState m_helperStackController;
    private HelperSearherTarget m_helperSearherTarget;

    //eatStates
    private HelperMoveToTarget m_helperMoveToEatTarget;
    private HelperEatState m_helperEatState;

    //poopStates
    private HelperMoveToTarget m_helperMoveToPoopTarget;
    private HelperDeaperState m_deaperState;

    //moveToSleepChild
    private HelperMoveToTarget m_helperMoveToChild;
    private ChildTransferState m_childTransferState;

    //ChilaEatStates
    private HelperStackControllerState m_helperStackEatChildControllerState;
    private FindSleepPlaceState m_findSleepPlaceState;
    private HelperMoveToTarget m_helperMoveToChildEatTarget;
    private HelperChildMoveToEatPlace m_helperChildMoveToEatPlace;

    //MoveToMother
    private HelperStackControllerState m_helperStackMother;
    private HelperMoveToTarget m_helperMoveToMother;
    private FindMotherState m_findMotherState;
    private MotherTakerState m_takerState;

    //sleepState
    private HelperStackControllerState m_helperSleep;
    private HelperMoveToTarget m_helperMoveToSleep;
    private HelperChildSleepState m_helperChildSleepState;
    private FindSleepSecondPlaceState m_findSleepSecondPlaceState;


    private bool isTargetfound;

    private StateMachine.StateMachine StateMachine;
    public Transform SearchTarget => searchTarget;
    public float SearchRadius => searchRadius;
    public HelperAnimatorController HelperAnimatorController => m_helperAnimatorController;

    private void Awake()
    {
        m_helperAnimatorController = new HelperAnimatorController(animator);
        InitializeStateMachine();
        StartCoroutine(SearchTargetCoroutine());
    }

    private void Update()
    {
        StateMachine.Tick();
        CurrentState = StateMachine.CurrentState.ToString();
    }

    private void InitializeStateMachine()
    {
        //search target
        m_helperIdleState = new HelperIdleState(m_helperAnimatorController);
        m_moveToPropState = new HelperMoveToTarget(m_helperAnimatorController, agent);
        m_helperStackController = new HelperStackControllerState(helperStack);
        m_helperSearherTarget = new HelperSearherTarget(this, helperStack);

        //search food
        m_helperMoveToEatTarget = new HelperMoveToTarget(m_helperAnimatorController, agent);
        m_helperEatState = new HelperEatState(helperStack);

        //serachDeaper
        m_deaperState = new HelperDeaperState(m_helperAnimatorController, transform, helperStack);
        m_helperMoveToPoopTarget = new HelperMoveToTarget(m_helperAnimatorController, agent);

        //moveToChildBeforeInitialize
        m_helperMoveToChild = new HelperMoveToTarget(m_helperAnimatorController, agent);
        m_childTransferState = new ChildTransferState(this);

        //eatChild
        m_helperStackEatChildControllerState = new HelperStackControllerState(helperStack);
        m_findSleepPlaceState = new FindSleepPlaceState(this);
        m_helperMoveToChildEatTarget = new HelperMoveToTarget(m_helperAnimatorController, agent);
        m_helperChildMoveToEatPlace = new HelperChildMoveToEatPlace(helperStack);

        //moveToMotherState
        m_helperMoveToMother = new HelperMoveToTarget(m_helperAnimatorController, agent);
        m_findMotherState = new FindMotherState(this);
        m_helperStackMother = new HelperStackControllerState(helperStack);
        m_takerState = new MotherTakerState(helperStack);

        //moveToSleepState
        m_helperSleep = new HelperStackControllerState(helperStack);
        m_findSleepSecondPlaceState = new FindSleepSecondPlaceState(this);
        m_helperMoveToSleep = new HelperMoveToTarget(m_helperAnimatorController, agent);
        m_helperChildSleepState = new HelperChildSleepState(helperStack);

        m_helperSleep.AddTransition(new StateTransition(m_findSleepSecondPlaceState,
            new FuncCondition(() => m_helperSleep.IsReadyToLeave)));

        m_helperMoveToSleep.AddTransition(new StateTransition(m_findSleepSecondPlaceState,
            new FuncCondition(() => m_helperChildSleepState.IsPlaceFree == false)));
        m_helperMoveToSleep.AddTransition(new StateTransition(m_helperChildSleepState,
            new FuncCondition(() => m_helperMoveToSleep.IsOnPosition)));

        m_helperChildSleepState.AddTransition(new StateTransition(m_helperIdleState, new FuncCondition(() =>
        {
            if (m_helperChildSleepState.IsReadyToLeave)
            {
                isTargetfound = false;
            }

            return m_helperChildSleepState.IsReadyToLeave;
        })));

        m_helperMoveToMother.AddTransition(new StateTransition(m_findMotherState,
            new FuncCondition(() => m_takerState.IsMotherFree == false)));
        m_helperMoveToMother.AddTransition(new StateTransition(m_takerState,
            new FuncCondition(() => m_helperMoveToMother.IsOnPosition)));

        m_takerState.AddTransition(new StateTransition(m_helperIdleState, new FuncCondition(() =>
        {
            if (m_takerState.IsReadyToLeave)
            {
                isTargetfound = false;
            }

            return m_takerState.IsReadyToLeave;
        })));

        m_helperStackMother.AddTransition(new StateTransition(m_findMotherState,
            new FuncCondition(() => m_helperStackMother.IsReadyToLeave)));

        m_helperStackEatChildControllerState.AddTransition(new StateTransition(m_findSleepPlaceState,
            new FuncCondition(() => m_helperStackEatChildControllerState.IsReadyToLeave)));

        m_helperMoveToChildEatTarget.AddTransition(new StateTransition(m_findSleepPlaceState,
            new FuncCondition(() => m_helperChildMoveToEatPlace.IsEatPlaceNonRead == false)));
        m_helperMoveToChildEatTarget.AddTransition(new StateTransition(m_helperChildMoveToEatPlace,
            new FuncCondition(() => m_helperMoveToChildEatTarget.IsOnPosition)));

        m_helperChildMoveToEatPlace.AddTransition(new StateTransition(m_helperIdleState, new FuncCondition(() =>
        {
            if (m_helperChildMoveToEatPlace.IsReadyToLeave)
            {
                isTargetfound = false;
            }

            return m_helperChildMoveToEatPlace.IsReadyToLeave;
        })));

        m_helperMoveToChild.AddTransition(new StateTransition(m_helperIdleState, new FuncCondition(() =>
        {
            if (m_childTransferState.IsChildFinish)
            {
                isTargetfound = false;
            }

            return m_childTransferState.IsChildFinish;
        })));
        m_helperMoveToChild.AddTransition(new StateTransition(m_childTransferState,
            new FuncCondition(() => m_helperMoveToChild.IsOnPosition)));

        m_helperMoveToPoopTarget.AddTransition(new StateTransition(m_helperSearherTarget,
            new FuncCondition(() => m_deaperState.IsChildReady)));
        m_helperMoveToPoopTarget.AddTransition(new StateTransition(m_deaperState,
            new FuncCondition(() => m_helperMoveToPoopTarget.IsOnPosition)));

        m_deaperState.AddTransition(new StateTransition(m_helperIdleState, new FuncCondition(() =>
        {
            if (m_deaperState.IsReadyToLeave)
            {
                isTargetfound = false;
            }

            return m_deaperState.IsReadyToLeave;
        })));

        m_helperSearherTarget.AddTransition(new StateTransition(m_helperIdleState, new FuncCondition(() =>
        {
            if (m_helperSearherTarget.IsReadyToLeave)
            {
                isTargetfound = false;
                helperStack.DropAllAttach();
            }

            return m_helperSearherTarget.IsReadyToLeave;
        })));

        m_helperMoveToEatTarget.AddTransition(new StateTransition(m_helperSearherTarget,
            new FuncCondition(() => m_helperEatState.IsChildReady)));
        m_helperMoveToEatTarget.AddTransition(
            new StateTransition(m_helperEatState, new FuncCondition(() => m_helperMoveToEatTarget.IsOnPosition))
        );
        m_helperEatState.AddTransition(new StateTransition(m_helperIdleState, new FuncCondition(() =>
        {
            if (m_helperEatState.IsReadyToLeave)
            {
                isTargetfound = false;
            }

            return m_helperEatState.IsReadyToLeave;
        })));

        StateMachine = new StateMachine.StateMachine(m_helperIdleState);


        m_moveToPropState.AddTransition(new StateTransition(m_helperIdleState,
            new FuncCondition(() =>
            {
                var returnValue = m_helperStackController.CurrentProp.IsAttach;
                if (returnValue)
                {
                    isTargetfound = false;
                }

                return returnValue;
            })));
        m_moveToPropState.AddTransition(new StateTransition(m_helperStackController,
            new FuncCondition(() => m_moveToPropState.IsOnPosition)));

        m_helperStackController.AddTransition(new StateTransition(m_helperSearherTarget,
            new FuncCondition(() => m_helperStackController.IsReadyToLeave)));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(searchTarget.position
            , searchRadius);
    }

    public void InitializeEatState(Child child)
    {
        m_helperEatState.SetChild(child);
        m_helperMoveToEatTarget.SetTarget(child.transform);
        StateMachine.SetState(m_helperMoveToEatTarget);
    }

    public void InitializePoopState(Child child)
    {
        m_helperMoveToPoopTarget.SetTarget(child.transform);
        m_deaperState.SetChild(child);

        StateMachine.SetState(m_helperMoveToPoopTarget);
    }

    private void InitializePropSearcher(Prop prop)
    {
        isTargetfound = true;
        m_moveToPropState.SetTarget(prop.transform);
        m_helperStackController.SetProp(prop);
        StateMachine.SetState(m_moveToPropState);
    }

    private void InitializeChildSearcher(Child child)
    {
        isTargetfound = true;
        m_helperMoveToChild.SetTarget(child.transform);
        m_childTransferState.Initialize(child);

        StateMachine.SetState(m_helperMoveToChild);
    }

    public void InitializeMoveToEat(Child child)
    {
        m_helperStackEatChildControllerState.SetChild(child);

        StateMachine.SetState(m_helperStackEatChildControllerState);
    }

    public void InitializeMoveToMother(Child child)
    {
        m_helperStackMother.SetChild(child);
        StateMachine.SetState(m_helperStackMother);
    }

    public void InitializeMoveToSleep(Child child)
    {
        m_helperSleep.SetChild(child);
        StateMachine.SetState(m_helperSleep);
    }

    public void SetEatPlace(EatPlace eatPlace)
    {
        m_helperMoveToChildEatTarget.SetTarget(eatPlace.transform);
        m_helperChildMoveToEatPlace.Initialize(eatPlace);

        StateMachine.SetState(m_helperMoveToChildEatTarget);
    }

    public void SetSleepPlace(ChildWantSleepPlace place)
    {
        m_helperMoveToSleep.SetTarget(place.transform);
        m_helperChildSleepState.Initialize(place);

        StateMachine.SetState(m_helperMoveToSleep);
    }

    public void SetMotherPlace(Mother motherPlace)
    {
        m_helperMoveToMother.SetTarget(motherPlace.transform);
        m_takerState.Initialize(motherPlace);

        StateMachine.SetState(m_helperMoveToMother);
    }

    private IEnumerator SearchTargetCoroutine()
    {
        while (true)
        {
            if (isTargetfound)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            else
            {
                yield return new WaitForSeconds(1f);
                LookingChild();
                if (isTargetfound == false)
                {
                    LookingProp();
                }
            }
        }
    }

    private void LookingProp()
    {
        var foundProp = Physics.OverlapSphere(searchTarget.position, searchRadius).Where(t =>
        {
            var prop = t.GetComponent<Prop>();
            if (isSecondHelper && prop && prop.PropType == PropType.Bottle) return false;
            if (isSecondHelper == false && prop && prop.PropType == PropType.Kasha) return false;
            if (prop == false || prop.IsAttach) return false;
            Debug.Log("Poop");
            switch (prop.PropType)
            {
                case PropType.Bottle:
                    var foundChild = Physics.OverlapSphere(SearchTarget.position, SearchRadius).Where(t =>
                    {
                        var child = t.GetComponent<Child>();
                        return child && child.FakeCurrentChildType == ChildStateType.Eat && (child.ChildOnEatPlace);
                    }).ToList();
                    return foundChild.Count != 0;
                    break;
                case PropType.DiaperProp:
                    var foundChildDiaper = Physics.OverlapSphere(SearchTarget.position, SearchRadius).Where(t =>
                    {
                        var child = t.GetComponent<Child>();
                        return child && child.CurrentChildType == ChildStateType.Poop;
                    }).ToList();
                    return foundChildDiaper.Count != 0;
                    break;
                case PropType.Kasha:
                    var foundChildKasha = Physics.OverlapSphere(SearchTarget.position, SearchRadius).Where(t =>
                    {
                        var child = t.GetComponent<Child>();
                        return child && child.FakeCurrentChildType == ChildStateType.Eat && child.ChildOnEatPlace;
                    }).ToList();
                    return foundChildKasha.Count != 0;
                    break;
            }

            return false;
        }).ToList();
        if (foundProp.Count != 0)
        {
            InitializePropSearcher(foundProp[0].GetComponent<
                Prop>());
        }
    }

    private void LookingChild()
    {
        var foundChild = Physics.OverlapSphere(searchTarget.position, searchRadius).Where(t =>
        {
            var child = t.GetComponent<Child>();
            if (child == false || child.IsSecondChild != isSecondHelper) return false;
            switch (child.FakeCurrentChildType)
            {
                case ChildStateType.Eat:
                    if (child.ChildOnEatPlace || child.GetComponent<StackableItem>().IsAttach) return false;
                    var foundObject = Physics.OverlapSphere(searchTarget.position, searchRadius).Where(t =>
                    {
                        var eapPlace = t.GetComponent<EatPlace>();
                        return eapPlace && eapPlace.IsPlaceFree;
                    }).ToList();
                    if (foundObject.Count == 0) return false;
                    return true;
                    break;
                case ChildStateType.Sleep:
                    if (child.ChildOnSleepPlace || child.GetComponent<StackableItem>().IsAttach) return false;
                    var foundObjectSleep = Physics.OverlapSphere(searchTarget.position, searchRadius).Where(t =>
                    {
                        var eapPlace = t.GetComponent<ChildWantSleepPlace>();
                        return eapPlace && eapPlace.IsPlaceFree;
                    }).ToList();
                    if (foundObjectSleep.Count == 0) return false;
                    return true;

                    break;
                case ChildStateType.ToMother:
                    if (child.ChildOnMotherPlace || child.GetComponent<StackableItem>().IsAttach) return false;
                    return true;
                    break;
            }

            return false;
        }).ToList();
        if (foundChild.Count != 0)
        {
            InitializeChildSearcher(foundChild[0].GetComponent<
                Child>());
        }
    }
}

public class HelperChildSleepState : State
{
    private readonly HelperStack m_helperStack;
    private ChildWantSleepPlace m_childWantSleepPlace;

    public bool IsPlaceFree => m_childWantSleepPlace.IsPlaceFree;
    public bool IsReadyToLeave;

    public HelperChildSleepState(HelperStack helperStack)
    {
        m_helperStack = helperStack;
    }

    public void Initialize(ChildWantSleepPlace childWantSleepPlace)
    {
        m_childWantSleepPlace = childWantSleepPlace;
    }

    public override void OnStateEnter()
    {
        var child = m_helperStack.DetachChild(ChildStateType.Sleep);
        m_childWantSleepPlace.Activate(child);
        IsReadyToLeave = true;
    }

    public override void OnStateExit()
    {
        IsReadyToLeave = false;
    }
}

public class HelperChildMoveToEatPlace : State
{
    private readonly HelperStack m_helperStack;
    private EatPlace m_eatPlace;

    public bool IsReadyToLeave;

    public bool IsEatPlaceNonRead => m_eatPlace.IsPlaceFree;

    public HelperChildMoveToEatPlace(HelperStack helperStack)
    {
        m_helperStack = helperStack;
    }

    public void Initialize(EatPlace eatPlace)
    {
        m_eatPlace = eatPlace;
    }

    public override void OnStateEnter()
    {
        var child = m_helperStack.DetachChild(ChildStateType.Eat);
        m_eatPlace.ActivateChild(child);
        child.ChildOnEatPlace = true;
        IsReadyToLeave = true;
    }

    public override void OnStateExit()
    {
        IsReadyToLeave = false;
    }
}

public class FindSleepPlaceState : State
{
    private readonly Helper m_helper;
    private float currentTick;

    public FindSleepPlaceState(Helper helper)
    {
        m_helper = helper;
    }

    public override void OnStateEnter()
    {
        Looking();
    }

    public override void Tick()
    {
        if (currentTick <= 0)
        {
            currentTick = 0.5f;
            Looking();
        }
        else
        {
            currentTick -= Time.deltaTime;
        }
    }


    private void Looking()
    {
        var foundObject = Physics.OverlapSphere(m_helper.SearchTarget.position, m_helper.SearchRadius).Where(t =>
        {
            var eapPlace = t.GetComponent<EatPlace>();
            return eapPlace && eapPlace.IsPlaceFree;
        }).ToList();
        if (foundObject.Count == 0) return;
        m_helper.SetEatPlace(foundObject[0].GetComponent<EatPlace>());
    }
}

public class FindSleepSecondPlaceState : State
{
    private readonly Helper m_helper;
    private float currentTick;

    public FindSleepSecondPlaceState(Helper helper)
    {
        m_helper = helper;
    }

    public override void OnStateEnter()
    {
        Looking();
    }

    public override void Tick()
    {
        if (currentTick <= 0)
        {
            currentTick = 0.5f;
            Looking();
        }
        else
        {
            currentTick -= Time.deltaTime;
        }
    }


    private void Looking()
    {
        var foundObject = Physics.OverlapSphere(m_helper.SearchTarget.position, m_helper.SearchRadius).Where(t =>
        {
            var eapPlace = t.GetComponent<ChildWantSleepPlace>();
            return eapPlace && eapPlace.IsPlaceFree;
        }).ToList();
        if (foundObject.Count == 0) return;
        m_helper.SetSleepPlace(foundObject[0].GetComponent<ChildWantSleepPlace>());
    }
}


public class MotherTakerState : State
{
    private readonly HelperStack m_helperStack;
    private Mother m_mother;
    public bool IsReadyToLeave;

    public bool IsMotherFree => m_mother.IsPlaceFree;

    public MotherTakerState(HelperStack helperStack)
    {
        m_helperStack = helperStack;
    }

    public void Initialize(Mother mother)
    {
        m_mother = mother;
    }

    public override void OnStateEnter()
    {
        IsReadyToLeave = true;
        var child = m_helperStack.DetachChild(ChildStateType.ToMother);
        child.ChildOnMotherPlace = true;
        m_mother.TakeChild(child, child.IsSecondChild);
    }

    public override void OnStateExit()
    {
        IsReadyToLeave = false;
    }
}

public class FindMotherState : State
{
    private readonly Helper m_helper;
    private float currentTick;

    public FindMotherState(Helper helper)
    {
        m_helper = helper;
    }

    public override void OnStateEnter()
    {
        Looking();
    }

    public override void Tick()
    {
        if (currentTick <= 0)
        {
            currentTick = 0.5f;
            Looking();
        }
        else
        {
            currentTick -= Time.deltaTime;
        }
    }


    private void Looking()
    {
        var foundObject = Physics.OverlapSphere(m_helper.SearchTarget.position, m_helper.SearchRadius).Where(t =>
        {
            var motherPlace = t.GetComponent<Mother>();
            return motherPlace && motherPlace.IsPlaceFree;
        }).ToList();
        if (foundObject.Count == 0) return;
        m_helper.SetMotherPlace(foundObject[0].GetComponent<Mother>());
    }
}

public class ChildTransferState : State
{
    private readonly Helper m_helper;
    private Child m_child;

    private ChildStateType currentType;
    public bool IsChildFinish => m_child.FakeCurrentChildType != currentType || m_child.StackableItem.IsAttach;

    public ChildTransferState(Helper helper)
    {
        m_helper = helper;
    }

    public void Initialize(Child child)
    {
        m_child = child;
        currentType = child.FakeCurrentChildType;
    }

    public override void OnStateEnter()
    {
        switch (m_child.FakeCurrentChildType)
        {
            case ChildStateType.Eat:
                m_helper.InitializeMoveToEat(m_child);
                break;
            case ChildStateType.ToMother:
                m_helper.InitializeMoveToMother(m_child);
                break;
            case ChildStateType.Sleep:
                m_helper.InitializeMoveToSleep(m_child);
                break;
        }
    }
}