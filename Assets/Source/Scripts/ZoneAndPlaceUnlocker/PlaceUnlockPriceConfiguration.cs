using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create PlaceUnlockPriceConfiguration",
    fileName = "PlaceUnlockPriceConfiguration", order = 0)]
public class PlaceUnlockPriceConfiguration : ScriptableObject
{
    [SerializeField] private int price;

    public int Price => price;
}