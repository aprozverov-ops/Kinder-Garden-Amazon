using UnityEngine;
using YG;

public class AwardingController : MonoBehaviour
{
    public static AwardingController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable() => YandexGame.RewardVideoEvent += Awarded;
    private void OnDisable() => YandexGame.RewardVideoEvent -= Awarded;

    private void Awarded(int id)
    {
        FreeWindowSystem.Instance.IssueAnAward(id);
    }
}