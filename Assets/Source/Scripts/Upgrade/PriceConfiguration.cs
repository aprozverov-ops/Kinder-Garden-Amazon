using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create PriceConfiguration", fileName = "PriceConfiguration", order = 0)]
public class PriceConfiguration : ScriptableObject
{
    [SerializeField] private PriceMakerConfiguration speedLevelConfiguration;
    [SerializeField] private PriceMakerConfiguration stackLevelConfiguration;
    [SerializeField] private PriceMakerConfiguration IncomeConfiguration;

    public PriceMakerConfiguration IncomeConfiguration1 => IncomeConfiguration;

    public PriceMakerConfiguration SpeedLevelConfiguration => speedLevelConfiguration;

    public PriceMakerConfiguration StackLevelConfiguration => stackLevelConfiguration;
}