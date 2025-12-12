using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PanelConfiguration
{
    [SerializeField] private Image button;
    [SerializeField] private Sprite enableButtonColor;
    [SerializeField] private Sprite disableButtonColor;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image panel;
    [SerializeField] private Sprite enable;
    [SerializeField] private Sprite disable;

    public void SetActivate(bool isActivate)
    {
        button.sprite = isActivate ? enableButtonColor : disableButtonColor;
        panel.sprite = isActivate ? enable : disable;
    }

    public void SetParam(string level, string price)
    {
        switch(LocalizationManager.Instance.GetCurrentLanguage())
        {
            case Languages.Russian:
                levelText.text = "ур. " + level;
                break;
            case Languages.English:
                levelText.text = "lvl " + level;
                break;
            case Languages.Turkish:
                levelText.text = "sev " + level;
                break;
        }

        priceText.text = price;
    }
}