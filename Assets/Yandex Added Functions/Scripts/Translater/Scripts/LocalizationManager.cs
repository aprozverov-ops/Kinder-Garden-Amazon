using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    [SerializeField] private Languages _currentLanguage;

    private List<LanguagePhraseChanger> _allPhraseChangers = new List<LanguagePhraseChanger>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddNewPhraseChangerOnList(LanguagePhraseChanger newPhraseChanger)
    {
        _allPhraseChangers.Add(newPhraseChanger);
    }

    public void SetNewLanguage(Languages newLanguage)
    {
        _currentLanguage = newLanguage;

        UpdateAllPhraseChangers();
    }

    private void UpdateAllPhraseChangers()
    {
        for (int i = 0; i < _allPhraseChangers.Count; i++)
        {
            _allPhraseChangers[i].ChangeLanguage(_currentLanguage);
        }
    }
 
    public Languages GetCurrentLanguage()
    {
        return _currentLanguage;
    }
}

public enum Languages
{
    Russian,
    English,
    Turkish,
}