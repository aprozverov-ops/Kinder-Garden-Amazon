using UnityEngine;

public class FocusSoundController : MonoBehaviour
{
    public static FocusSoundController Instance { get; private set; }

    private bool _muteAllSounds;

    private void Awake()
    {
        Instance = this;
    }

    public bool GetMuteState()
    {
        return _muteAllSounds;
    }

    public void MuteAllSounds()
    {
        _muteAllSounds = true;

        Silence(true);
    }

    public void UnmuteAllSounds()
    {
        _muteAllSounds = false;

        Silence(false);
    }

    private void Silence(bool silence)
    {
        AudioListener.pause = silence;
        AudioListener.volume = silence ? 0 : 1;
    }
}