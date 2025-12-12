using System.Collections.Generic;
using UnityEngine;

public class ChildEffectActivator : MonoBehaviour
{
    [SerializeField] private List<ChildEffectConfiguration> configurations;

    public void ActivateEffect(ChildEffectType effectType, bool isActivate)
    {
        foreach (var cfg in configurations)
        {
            if (cfg.ChildEffectType == effectType) cfg.EffectObject.SetActive(isActivate);
        }
    }
}