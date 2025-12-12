using Kuhpik;
using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create CharacterConfigurations", fileName = "CharacterConfigurations",
    order = 0)]
public class CharacterConfigurations : ScriptableObject
{
    [SerializeField] private float addSpeedPerLevel = 100f;
    [SerializeField] private float increaseSpeedPerLevel = 0.4f;

    [Header("0 = 0%, 1=100%")] [Range(0f, 1f)] [SerializeField]
    private float addMoneyPercentPerLevel;

    public float AddMoneyPercentPerLevel => addMoneyPercentPerLevel;

    public float IncreaseSpeedPerLevel => increaseSpeedPerLevel;

    public float AddSpeedPerLevel => addSpeedPerLevel;

    [field: SerializeField] public float Speed { get; private set; }

    [field: SerializeField] public float SpeedRotate { get; private set; }

    public float GenerationSpeed(float minSpeed, float maxSpeed)
    {
        minSpeed = minSpeed - increaseSpeedPerLevel * Bootstrap.Instance.PlayerData.UpgadeLevel[UpgradeType.Speed] - 1;
        if (minSpeed < 0) minSpeed = 0;
        maxSpeed = maxSpeed - increaseSpeedPerLevel * Bootstrap.Instance.PlayerData.UpgadeLevel[UpgradeType.Speed] - 1;
        if (maxSpeed < 0.5f) maxSpeed = 0.5f;
        Debug.Log(maxSpeed + "" + minSpeed);
        return Random.Range(minSpeed, maxSpeed);
    }
}