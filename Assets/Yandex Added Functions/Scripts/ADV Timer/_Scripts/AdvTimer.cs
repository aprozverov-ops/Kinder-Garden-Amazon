using System.Collections;
using UnityEngine;
using TMPro;
using YG;

public class AdvTimer : MonoBehaviour
{
    [SerializeField] private float _timeBetweenShowFullscreenAdv = 90f;

    [SerializeField] private GameObject _pauseScreen;

    [SerializeField] private TextMeshProUGUI _timerText;

    [SerializeField] private float _timeForShowAdvTimer = 4;

    private float _currentTimerSize;

    private bool _isAdvReady;

    private void Start()
    {
        StartCoroutine(ShowFullscreenAdvWithDelay());
    }

    private void Update()
    {
        if (_isAdvReady == true)
        {
            _currentTimerSize -= 0.03f;

            switch (LocalizationManager.Instance.GetCurrentLanguage())
            {
                case Languages.Russian:
                    _timerText.text = "Реклама будет показана через " + Mathf.RoundToInt(_currentTimerSize) + "...";
                    break;
                case Languages.English:
                    _timerText.text = "Ads will be shown through " + Mathf.RoundToInt(_currentTimerSize) + "...";
                    break;
                case Languages.Turkish:
                    _timerText.text = "Reklamlar aracılığıyla gösterilecek " + Mathf.RoundToInt(_currentTimerSize) + "...";
                    break;
            }

            if (_currentTimerSize <= 0)
            {
                YandexGame.FullscreenShow();

                _pauseScreen.SetActive(false);

                StartCoroutine(ShowFullscreenAdvWithDelay());

                _isAdvReady = false;

                Time.timeScale = 1;
            }
        }
    }

    private IEnumerator ShowFullscreenAdvWithDelay()
    {
        _currentTimerSize = _timeForShowAdvTimer;
        yield return new WaitForSeconds(_timeBetweenShowFullscreenAdv);
        _pauseScreen.SetActive(true);
        _isAdvReady = true;
        Time.timeScale = 0;
    }
}