using Kuhpik;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

public class VibrationSystem : GameSystem
{
    [SerializeField] private Image vibrationImage;
    [SerializeField] private Sprite enableSprite;
    [SerializeField] private Sprite disableSprite;

    public override void OnInit()
    {
        SetVibration();
    }

    public void ChangeVibration()
    {
        player.IsVibration = !player.IsVibration;
        SetVibration();
    }
    public static void Play()
    {
        MMVibrationManager.Haptic(HapticTypes.SoftImpact);
    }

    public void SetVibration()
    {
        MMVibrationManager.SetHapticsActive(player.IsVibration);
        vibrationImage.sprite = player.IsVibration ? enableSprite : disableSprite;
    }
}