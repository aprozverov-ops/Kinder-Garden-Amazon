using Kuhpik;
using UnityEngine;

public class MoneySpawnUISystem : GameSystem
{
    [SerializeField] private UIMoney moneyPrefab;

    private IPool<UIMoney> moneyPool;

    private void Awake()
    {
        var factory = new FactoryMonoObject<UIMoney>(moneyPrefab.gameObject, transform);
        moneyPool = new Pool<UIMoney>(factory, 3);
    }

    public void SpawnMoney(int amount, Transform pos)
    {
        var effect = moneyPool.Pull();
        effect.transform.position = new Vector3(pos.position.x, 1.26f, pos.position.z);
        effect.Show(amount);
    }
}