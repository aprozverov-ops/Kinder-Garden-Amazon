using System.Collections.Generic;
using System.Linq;
using Kuhpik;
using UnityEngine;

public class ChildPositionController : GameSystem
{
    [SerializeField] private List<ChildPlayConfiguration> childPlayConfigurations;
    [SerializeField] private List<ChildPlayConfiguration> childPlaySecondConfigurations;

    public ChildPlayConfiguration GetRandomPlayPosition(bool isSecondChild)
    {
        var chile = isSecondChild
            ? childPlaySecondConfigurations.Where(t => t.IsActivate).ToList()
            : childPlayConfigurations.Where(t => t.IsActivate).ToList();
        return chile[Random.Range(0, chile.Count)];
    }
}