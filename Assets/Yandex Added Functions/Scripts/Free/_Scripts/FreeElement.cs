using UnityEngine;
using UnityEngine.UI;
using YG;
using TMPro;

public class FreeElement : MonoBehaviour
{
    [Header("Resource name")]
    [SerializeField] private string _resourceName;

    [Header("Reward count & Multiplier settings")]
    [SerializeField] private int _countMoneyForViewAdv;

    private int _countMoneyForReward;
    public int CountMoneyForReward => _countMoneyForReward;

    [SerializeField] private float _rewardIncreaseMultiplier;

    [Header("Counter Text Field")]
    [SerializeField] private TextMeshProUGUI _counterText;

    [Header("Timer Settings")]
    [SerializeField] private TextMeshProUGUI _getButtonTextField;

    [SerializeField] private bool _isEnableTimer = true;

    [SerializeField] private float _timerSize = 60f;

    private float _currentTimerSize;

    private bool _isTimerEnd = true;

    [Header("Get Button Settings")]
    [SerializeField] private Image _buttonImageComponent;

    [SerializeField] private Button _buttonComponent;

    [SerializeField] private Sprite _enableButtonSprite;

    [SerializeField] private Sprite _disableButtonSprite;

    private void Start()
    {
        if (PlayerPrefs.HasKey(_resourceName + " Free"))
        {
            _countMoneyForViewAdv = PlayerPrefs.GetInt(_resourceName + " Free");
        }

        _countMoneyForReward = _countMoneyForViewAdv;

        _counterText.text = "x" + _countMoneyForReward;

        switch (LocalizationManager.Instance.GetCurrentLanguage())
        {
            case Languages.Russian:
                _getButtonTextField.text = "онксвхрэ";
                break;
            case Languages.English:
                _getButtonTextField.text = "GET";
                break;
            case Languages.Turkish:
                _getButtonTextField.text = "ELDE ETMEK";
                break;
        }
    }

    private void Update()
    {
        if(_isEnableTimer == false)
        {
            return;
        }

        if(_isTimerEnd == false)
        {
            _currentTimerSize -= Time.deltaTime;

            _getButtonTextField.text = "0:" + Mathf.RoundToInt(_currentTimerSize);

            if(_currentTimerSize <= 0)
            {
                _isTimerEnd = true;

                _buttonImageComponent.sprite = _enableButtonSprite;

                _buttonComponent.interactable = true;

                switch (LocalizationManager.Instance.GetCurrentLanguage())
                {
                    case Languages.Russian:
                        _getButtonTextField.text = "онксвхрэ";
                        break;
                    case Languages.English:
                        _getButtonTextField.text = "GET";
                        break;
                    case Languages.Turkish:
                        _getButtonTextField.text = "ELDE ETMEK";
                        break;
                }
            }
        }
    }

    public void GetReward()
    {
        if(_isTimerEnd == false)
        {
            return;
        }

        switch(_resourceName)
        {
            case "Money":
                YandexGame.RewVideoShow(0);
                break;
            case "Coal":
                YandexGame.RewVideoShow(1);
                break;
            case "Tree":
                YandexGame.RewVideoShow(2);
                break;
        }
    }

    public void CalculateNewRewardCount()
    {
        _countMoneyForViewAdv = Mathf.RoundToInt(_countMoneyForViewAdv * _rewardIncreaseMultiplier);

        PlayerPrefs.SetInt(_resourceName + " Free", _countMoneyForViewAdv);

        _countMoneyForReward = _countMoneyForViewAdv;

        _counterText.text = "x" + _countMoneyForReward;

        if(_isEnableTimer == true)
        {
            _currentTimerSize = _timerSize;

            _isTimerEnd = false;

            _buttonImageComponent.sprite = _disableButtonSprite;

            _buttonComponent.interactable = false;
        }
    }
}