using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create ChildConfiguration", fileName = "ChildConfiguration", order = 0)]
public class ChildConfiguration : ScriptableObject
{
    [BoxGroup("Poop")] [SerializeField] private float minPoopTime;
    [BoxGroup("Poop")] [SerializeField] private float maxPoopTime;

    [BoxGroup("Eat")] [SerializeField] private float minEatTime;
    [BoxGroup("Eat")] [SerializeField] private float maxEatTime;
    
    [BoxGroup("Sleep")] [SerializeField] private float minSleepTime;
    [BoxGroup("Sleep")] [SerializeField] private float maxSleepTime;

    public float MINSleepTime => minSleepTime;

    public float MAXSleepTime => maxSleepTime;

    public float MINEatTime => minEatTime;

    public float MAXEatTime => maxEatTime;

    public float MINPoopTime => minPoopTime;

    public float MAXPoopTime => maxPoopTime;
}