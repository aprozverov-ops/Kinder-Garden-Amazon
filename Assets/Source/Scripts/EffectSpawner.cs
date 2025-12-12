using System.Collections.Generic;
using System.Linq;
using Kuhpik;
using UnityEngine;

public class EffectSpawner : GameSystem
{
    [SerializeField] private List<EffectConfiguration> effectConfigurations;

    private Dictionary<EffectType, IPool<TemporaryMonoPooled>> effects =
        new Dictionary<EffectType, IPool<TemporaryMonoPooled>>();

    public void SpawnEffect(EffectType effectType, Transform position)
    {
        TryToInitializeEffect(effectType);
        var newEffect = effects[effectType].Pull();
        newEffect.transform.position = position.position;
        newEffect.transform.rotation = position.rotation;
    }

    public Transform SpawnEffect(EffectType effectType, Transform attach, bool isAttach = true)
    {
        TryToInitializeEffect(effectType);
        var newEffect = effects[effectType].Pull();
        newEffect.transform.parent = attach;
        newEffect.transform.localPosition = new Vector3(0, 1.214f, 0);
        newEffect.transform.localRotation = Quaternion.identity;

        return newEffect.transform;
    }

    private void TryToInitializeEffect(EffectType effectType)
    {
        if (effects.ContainsKey(effectType))
        {
            return;
        }

        var effect = effectConfigurations.Where(t => t.EffectType == effectType).ToList();
        if (effect.Count == 0)
        {
            Debug.LogError($"Effect {effectType} not initialized");
            return;
        }

        var factoryMono = new FactoryMonoObject<TemporaryMonoPooled>(effect[0].Effect.gameObject, transform);
        effects.Add(effectType, new Pool<TemporaryMonoPooled>(factoryMono, 1));
    }
}