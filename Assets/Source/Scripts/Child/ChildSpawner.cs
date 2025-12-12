using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class ChildSpawner : GameSystem
{
    [SerializeField] private List<Child> childrenPrefabsFirst;
    [SerializeField] private List<Child> childrenPrefabsSecond;

    private readonly List<IPool<Child>> m_childPoolsFirst = new List<IPool<Child>>();
    private readonly List<IPool<Child>> m_childPoolsSecond = new List<IPool<Child>>();

    public override void OnInit()
    {
        foreach (var child in childrenPrefabsFirst)
        {
            var factory = new FactoryMonoObject<Child>(child.gameObject, transform);
            var pool = new Pool<Child>(factory, 1);
            m_childPoolsFirst.Add(pool);
        }
        
        foreach (var child in childrenPrefabsSecond)
        {
            var factory = new FactoryMonoObject<Child>(child.gameObject, transform);
            var pool = new Pool<Child>(factory, 1);
            m_childPoolsSecond.Add(pool);
        }
    }

    public Child SpawnNewChild(bool isSecond)
    {
        if (isSecond == false)
        {
            game.amountChildFirst++;
            var newChild = m_childPoolsFirst[Random.Range(0, m_childPoolsFirst.Count)].Pull();
            return newChild;
        }
        else
        {
            game.amountChildSecond++;
            var newChild = m_childPoolsSecond[Random.Range(0, m_childPoolsSecond.Count)].Pull();
            return newChild;
        }
    }
}