using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuItem : MonoBehaviour
{
    [HideInInspector] public Image ImageComponent;
    [HideInInspector] public Transform TransformComponent;

    private SettingsMenu _settingsMenu;
    private Button _button;
    private int _index;

    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;

    private void Awake()
    {
        ImageComponent = GetComponent<Image>();

        TransformComponent = transform;

        _settingsMenu = TransformComponent.parent.GetComponent<SettingsMenu>();

        _index = TransformComponent.GetSiblingIndex() - 1;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnItemClick);
    }

    private void OnItemClick()
    {
        _settingsMenu.OnItemClick(_index);

        if(_onSprite == null || _offSprite == null)
        {
            return;
        }

        if(FocusSoundController.Instance.GetMuteState() == false)
        {
            ImageComponent.sprite = _onSprite;
        }
        else
        {
            ImageComponent.sprite = _offSprite;
        }
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnItemClick);
    }
}