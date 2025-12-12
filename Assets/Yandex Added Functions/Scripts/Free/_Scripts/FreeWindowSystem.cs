using EventBusSystem;
using Kuhpik;
using Snippets.Tutorial;
using UnityEngine;

public class FreeWindowSystem : GameSystem
{
    [Header("All free elements")]
    [SerializeField] private FreeElement[] _allElements;

    [Header("Window Object")]
    [SerializeField] private GameObject _freeMoneyWindow;

    [Header("Free Menu Open Button")]
    [SerializeField] private GameObject _freeButton;

    public static FreeWindowSystem Instance { get; private set; }

    private void Awake()
    {
        TutorialSystem.EndTutorial.AddListener(ActivateButton);

        Instance = this;
    }

    public void CallWindow()
    {
        _freeMoneyWindow.SetActive(true);
    }

    public void HideWindow()
    {
        _freeMoneyWindow.SetActive(false);
    }

    public void IssueAnAward(int number)
    {
        switch(number)
        {
            case 0:
                Bootstrap.Instance.PlayerData.Money += _allElements[number].CountMoneyForReward;
                EventBusSystem.EventBus.RaiseEvent<IUpdateMoney>(t => t.UpdateMoney());

                _allElements[number].CalculateNewRewardCount();
                break;
        }

        Bootstrap.Instance.SaveGame();
    }

    private void ActivateButton()
    {
        _freeButton.SetActive(true);
    }
}