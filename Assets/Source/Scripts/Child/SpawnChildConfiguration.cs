using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create SpawnChildConfiguration", fileName = "SpawnChildConfiguration", order = 0)]
public class SpawnChildConfiguration : ScriptableObject
{
    [SerializeField] private int amountChildInFirstZone;
    [SerializeField] private int amountChildInSecondZone;

    public int AmountChildInFirstZone => amountChildInFirstZone;

    public int AmountChildInSecondZone => amountChildInSecondZone;
}