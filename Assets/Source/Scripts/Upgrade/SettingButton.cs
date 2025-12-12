using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private List<Image> Image;
    [SerializeField] private Image button;

    private bool isActivate;

    public void Activate()
    {
        if (isActivate)
        {
            foreach (var image in Image)
            {
                image.DOFade(0, 0.4f);
            }

            button.DOFade(0, 0.4f).OnComplete(() => button.raycastTarget = false);
            isActivate = false;
        }
        else
        {
            foreach (var image in Image)
            {
                image.DOFade(1, 0.4f);
            }

            button.DOFade(1, 0.4f).OnComplete(() => button.raycastTarget = true);
            isActivate = true;
        }
    }
}