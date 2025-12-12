using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsMenu : MonoBehaviour
{
    [Header("Space between menu items")]
    [SerializeField] private Vector2 _spacing;

    [Space]
    [Header("Main button rotation")]
    [SerializeField] private float _rotationDuration;
    [SerializeField] Ease _rotationEase;

    [Space]
    [Header("Animation")]
    [SerializeField] private float _expandDuration;
    [SerializeField] private float _collapseDuration;
    [SerializeField] Ease _expandEase;
    [SerializeField] Ease _collapseEase;

    [Space]
    [Header("Fading")]
    [SerializeField] private float _expandFadeDuration;
    [SerializeField] private float _collapseFadeDuration;

    private Button _mainButton;
    private SettingsMenuItem[] _menuItems;

    private bool _isExpanded = false;

    private Vector2 _mainButtonPosition;
    private int _itemsCount;

    private void Start()
    {
        _itemsCount = transform.childCount - 1;

        _menuItems = new SettingsMenuItem[_itemsCount];

        for (int i = 0; i < _itemsCount; i++)
        {
            _menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }

        _mainButton = transform.GetChild(0).GetComponent<Button>();

        _mainButton.onClick.AddListener(ToggleMenu);

        _mainButton.transform.SetAsLastSibling();

        _mainButtonPosition = _mainButton.transform.position;

        ResetPosition();
    }

    private void ResetPosition()
    {
        for (int i = 0; i < _itemsCount; i++)
        {
            _menuItems[i].TransformComponent.position = _mainButtonPosition;
        }
    }

    private void ToggleMenu()
    {
        _isExpanded = !_isExpanded;

        if(_isExpanded == true)
        {
            for (int i = 0; i < _itemsCount; i++)
            {
                _menuItems[i].TransformComponent.DOMove(_mainButtonPosition + _spacing * (i + 1), _expandDuration).SetEase(_expandEase);

                _menuItems[i].ImageComponent.DOFade(1f, _expandFadeDuration).From(0f);
            }
        }
        else
        {
            for (int i = 0; i < _itemsCount; i++)
            {
                _menuItems[i].TransformComponent.DOMove(_mainButtonPosition, _collapseDuration).SetEase(_collapseEase);

                _menuItems[i].ImageComponent.DOFade(0f, _collapseFadeDuration);
            }
        }

        _mainButton.transform.DORotate(Vector3.forward * 180f, _rotationDuration).From(Vector3.zero).SetEase(_rotationEase);
    }

    public void OnItemClick(int index)
    {
        switch(index)
        {
            case 0:
                if (FocusSoundController.Instance.GetMuteState() == false)
                {
                    FocusSoundController.Instance.MuteAllSounds();
                }
                else
                {
                    FocusSoundController.Instance.UnmuteAllSounds();
                }
                break;
            case 1:

                break;
        }
    }

    private void OnDestroy()
    {
        _mainButton.onClick.RemoveListener(ToggleMenu);
    }
}