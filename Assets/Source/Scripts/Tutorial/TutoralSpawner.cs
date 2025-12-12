using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EventBusSystem;
using Kuhpik;
using Snippets.Tutorial;
using StateMachine;
using UnityEngine;

public class TutoralSpawner : GameSystem
{
    [SerializeField] private GameObject mother;
    [SerializeField] private GameObject child;

    [SerializeField] private Transform motherSpawnPos;
    [SerializeField] private Transform motherEndPos;
    [SerializeField] private Transform childSpawnPos;

    [SerializeField] private GameObject tutorialU1;
    [SerializeField] private GameObject tutorialU2;
    [SerializeField] private GameObject tutorialClose;

    [SerializeField] private GameObject deaperProp;
    [SerializeField] private GameObject earProp;
    [SerializeField] private GameObject props;

    [SerializeField] private GameObject upgrade;
    [SerializeField] private GameObject buy;

    [SerializeField] private GameObject tutorialPoof_1;
    [SerializeField] private GameObject tutorialPoof_2;

    public GameObject TutorialPoof1 => tutorialPoof_1;

    public GameObject TutorialPoof2 => tutorialPoof_2;

    public GameObject Upgrade => upgrade;

    public GameObject Buy => buy;

    public GameObject Props => props;

    public GameObject DeaperProp => deaperProp;

    public GameObject EarProp => earProp;

    public GameObject TutorialU1 => tutorialU1;

    public GameObject TutorialU2 => tutorialU2;

    public GameObject TutorialClose => tutorialClose;

    public GameObject UpgradePlace;

    public override void OnInit()
    {
        if (player.IsTutorialFinish == false)
        {
            var mother = Instantiate(this.mother);
            mother.transform.position = motherSpawnPos.position;
            mother.transform.rotation = motherSpawnPos.rotation;
            mother.GetComponent<Mother>().MotherTutorial(motherSpawnPos, motherEndPos);

            var child = Instantiate(this.child);
            child.transform.position = childSpawnPos.position;
            child.transform.rotation = childSpawnPos.rotation;
            child.GetComponent<Child>().WantToPoop();
        }
    }
}

[Serializable]
public class TutorialFirstStart : TutorialStep
{
    public override void OnUpdate()
    {
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<UpgradeInitializer>().Coroutine(StartGame());
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(1, "child_want_poop");
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject.FindObjectOfType<TutoralSpawner>().Props.SetActive(false);
        GameObject.FindObjectOfType<TutoralSpawner>().Upgrade.SetActive(false);
        GameObject.FindObjectOfType<TutoralSpawner>().Buy.transform.localScale = Vector3.zero;
        Bootstrap.Instance.GetSystem<TutoralSpawner>().DeaperProp.transform.localScale = Vector3.zero;
        Bootstrap.Instance.GetSystem<TutoralSpawner>().EarProp.transform.localScale = Vector3.zero;
        Bootstrap.Instance.ChangeGameState(GameStateID.Pause);
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Child);
        yield return new WaitForSeconds(2.8f);
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Poop);
        yield return new WaitForSeconds(2.2f);
        Bootstrap.Instance.GetSystem<TutoralSpawner>().DeaperProp.transform.localScale = Vector3.one;
        Bootstrap.Instance.GetSystem<TutoralSpawner>().TutorialPoof1.SetActive(true);
        Bootstrap.Instance.GetSystem<TutoralSpawner>().DeaperProp.transform.DOShakeScale(0.5f, 0.5f);
        yield return new WaitForSeconds(1f);
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Walk);
        Bootstrap.Instance.ChangeGameState(GameStateID.Game);
        Complete();
    }
}

[Serializable]
public class TutorialMoveToPooper : TutorialStep
{
    private Prop prop;

    public override void OnUpdate()
    {
        if (prop.IsAttach) Complete();
    }

    protected override void OnBegin()
    {
        prop = GameObject.FindObjectOfType<Prop>();
        TutorialArrow.Instance.SetTarget(prop.transform);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(2, "move_to_diaper");
        TutorialArrow.Instance.SetTarget(null);
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialMoveToPoopChild : TutorialStep
{
    private Stack stack;

    public override void OnUpdate()
    {
        if (stack.IsCanBeDetachProp(PropType.DiaperProp) == false) Complete();
    }

    protected override void OnBegin()
    {
        stack = GameObject.FindObjectOfType<Stack>();
        TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Child>().transform);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(3, "diaper_change");
        TutorialArrow.Instance.SetTarget(null);
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialCameraToBed : TutorialStep
{
    public override void OnUpdate()
    {
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<UpgradeInitializer>().Coroutine(Start());
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(4, "camera_to_bed");
    }

    private IEnumerator Start()
    {
        GameObject.FindObjectOfType<Stack>().PauseStack = true;
        yield return new WaitForSeconds(1f);
        Bootstrap.Instance.ChangeGameState(GameStateID.Pause);
        yield return new WaitForSeconds(3f);
        GameObject.FindObjectOfType<Stack>().PauseStack = false;
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Bed);
        yield return new WaitForSeconds(2.8f);
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Walk);
        Bootstrap.Instance.ChangeGameState(GameStateID.Game);
        Complete();
    }
}

[Serializable]
public class TutorialMoveToBed : TutorialStep
{
    private EatPlace m_eatPlace;

    public override void OnUpdate()
    {
        if (m_eatPlace.IsPlaceFree == false) Complete();
    }

    protected override void OnBegin()
    {
        m_eatPlace = GameObject.FindObjectsOfType<EatPlace>().Where(t => t.IsSecond == false).ToList()[0];
        TutorialArrow.Instance.SetTarget(m_eatPlace.transform);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(5, "move_to_bed");
        TutorialArrow.Instance.SetTarget(null);
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutoralCameraToEat : TutorialStep
{
    public override void OnUpdate()
    {
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<UpgradeInitializer>().Coroutine(Start());
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(6, "camera_to_eat_place");
    }

    private IEnumerator Start()
    {
        Bootstrap.Instance.ChangeGameState(GameStateID.Pause);
        yield return new WaitForSeconds(0.6f);
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Eat);
        yield return new WaitForSeconds(2.8f);
        Bootstrap.Instance.GetSystem<TutoralSpawner>().EarProp.transform.localScale = Vector3.one;
        Bootstrap.Instance.GetSystem<TutoralSpawner>().TutorialPoof2.SetActive(true);
        Bootstrap.Instance.GetSystem<TutoralSpawner>().EarProp.transform.DOShakeScale(0.5f, 0.5f);
        yield return new WaitForSeconds(1f);
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Walk);
        Bootstrap.Instance.ChangeGameState(GameStateID.Game);
        Complete();
    }
}

[Serializable]
public class TutorialMoveToFood : TutorialStep
{
    private Prop prop;

    public override void OnUpdate()
    {
        if (prop.IsAttach) Complete();
    }

    protected override void OnBegin()
    {
        prop = GameObject.FindObjectOfType<Prop>();
        TutorialArrow.Instance.SetTarget(prop.transform);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(7, "move_to_eat_place");
        TutorialArrow.Instance.SetTarget(null);
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialMoveToBedWithFood : TutorialStep
{
    private EatPlace m_eatPlace;
    private Stack m_stack;

    public override void OnUpdate()
    {
        if (m_stack.IsCanBeDetachProp(PropType.Bottle) == false) Complete();
    }

    protected override void OnBegin()
    {
        m_eatPlace = GameObject.FindObjectsOfType<EatPlace>().Where(t => t.IsSecond == false).ToList()[0];
        TutorialArrow.Instance.SetTarget(m_eatPlace.transform);
        m_stack = GameObject.FindObjectOfType<Stack>();
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(8, "give_food_to_child");
        TutorialArrow.Instance.SetTarget(null);
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialWaitSleep : TutorialStep
{
    private Child m_child;
    private bool isa;

    public override void OnUpdate()
    {
        if (m_child.CurrentChildType == ChildStateType.ToMother)
        {
            if (isa == false)
            {
                isa = true;
                TutorialArrow.Instance.SetTarget(m_child.transform);
                TutorialArrow.Instance.ShowArrow();
            }
        }

        if (m_child.StackableItem.IsAttach) Complete();
    }

    protected override void OnBegin()
    {
        m_child = GameObject.FindObjectOfType<Child>();
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(9, "sleep");
        TutorialArrow.Instance.SetTarget(null);
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialMoveToParent : TutorialStep
{
    private Mother m_mother;

    public override void OnUpdate()
    {
        if (m_mother.Child != null) Complete();
    }

    protected override void OnBegin()
    {
        m_mother = GameObject.FindObjectOfType<Mother>();
        TutorialArrow.Instance.SetTarget(m_mother.transform);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(10, "move_to_parent");
        TutorialArrow.Instance.SetTarget(null);
        TutorialArrow.Instance.DisableArrow();
        Bootstrap.Instance.PlayerData.Money = 10;
        EventBus.RaiseEvent<IUpdateMoney>(t => t.UpdateMoney());
    }
}

[Serializable]
public class MoveToUpgradePlace : TutorialStep
{
    public override void OnUpdate()
    {
        if (Bootstrap.Instance.GetCurrentGamestateID() == GameStateID.Menu)
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutoralSpawner>().Upgrade.SetActive(true);
        TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<TutoralSpawner>().UpgradePlace.transform);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(11, "open_upgrade_menu");
        TutorialArrow.Instance.SetTarget(null);
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class BuyFirstPlace : TutorialStep
{
    public override void OnUpdate()
    {
        if (Bootstrap.Instance.PlayerData.UpgadeLevel[UpgradeType.Income] == 2) Complete();
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutoralSpawner>().TutorialU1.SetActive(true);
        GameObject.FindObjectOfType<TutoralSpawner>().TutorialClose.SetActive(false);
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(12, "buy_first_upgrade");
        GameObject.FindObjectOfType<TutoralSpawner>().TutorialU1.SetActive(false);
    }
}

[Serializable]
public class BuySecondPlace : TutorialStep
{
    public override void OnUpdate()
    {
        if (Bootstrap.Instance.GetCurrentGamestateID() == GameStateID.Game)
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutoralSpawner>().TutorialClose.SetActive(true);
        GameObject.FindObjectOfType<TutoralSpawner>().TutorialU2.SetActive(true);
    }

    protected override void OnComplete()
    {
        Bootstrap.Instance.PlayerData.TryToSaveTutorSteps(13, "close_upgrade_menu");
        GameObject.FindObjectOfType<TutoralSpawner>().TutorialU2.SetActive(false);
        GameObject.FindObjectOfType<TutoralSpawner>().Props.SetActive(true);
        GameObject.FindObjectOfType<TutoralSpawner>().Buy.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
        Bootstrap.Instance.PlayerData.IsTutorialFinish = true;
        Bootstrap.Instance.SaveGame();
    }
}