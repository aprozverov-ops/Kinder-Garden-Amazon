using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChildSpriteRotator : MonoBehaviour
{
    [SerializeField] private Image image;

    public bool IsReadyToLeave;
    public bool IsEnabe;
    private void Awake()
    {
        Disable();
    }

    public void Activate(float time)
    {
        IsEnabe = true;
        image.enabled = true;
        image.fillAmount = 0;
        image.DOFillAmount(1, time).OnComplete(() => IsReadyToLeave = true);
    }

    public void Disable()
    {
        IsEnabe = false;
        image.fillAmount = 0;
        image.enabled = false;
        IsReadyToLeave = false;
    }
}