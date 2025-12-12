using UnityEngine;
using UnityEngine.UI;

public class MuteButtonController : MonoBehaviour
{
    [SerializeField] private Image _imageController;

    [SerializeField] private Sprite _muteButtonSprite;
    [SerializeField] private Sprite _unmuteButtonSprite;

    public void ClickOnButton()
    {
        switch(FocusSoundController.Instance.GetMuteState())
        {
            case true:
                FocusSoundController.Instance.UnmuteAllSounds();
                _imageController.sprite = _unmuteButtonSprite;
                break;
            case false:
                FocusSoundController.Instance.MuteAllSounds();
                _imageController.sprite = _muteButtonSprite;
                break;
        }
    }
}