using UnityEngine;
using TMPro;

public class LanguagePhraseChanger : MonoBehaviour
{
    private TextMeshProUGUI _textField;

    [Header("Russian translate")]
    [TextArea] public string RussianPhrase;

    [Header("English translate")]
    [TextArea] public string EnglishPhrase;

    [Header("Turkish translate")]
    [TextArea] public string TurkishPhrase;

    private void Awake()
    {
        _textField = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        ChangeLanguage(LocalizationManager.Instance.GetCurrentLanguage());

        LocalizationManager.Instance.AddNewPhraseChangerOnList(this);
    }

    public void ChangeLanguage(Languages newLanguage)
    {
        if(_textField == null)
        {
            _textField = GetComponent<TextMeshProUGUI>();
        }

        switch(newLanguage)
        {
            case Languages.Russian:
                _textField.text = RussianPhrase;
                break;
            case Languages.English:
                _textField.text = EnglishPhrase;
                break;
            case Languages.Turkish:
                _textField.text = TurkishPhrase;
                break;
        }
    }
}
