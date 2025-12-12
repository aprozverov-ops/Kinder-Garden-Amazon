using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create MotherConfiguration", fileName = "MotherConfiguration", order = 0)]
public class MotherConfiguration : ScriptableObject
{
    [SerializeField] private int moneyPerChild;

    public int Money => moneyPerChild;
}