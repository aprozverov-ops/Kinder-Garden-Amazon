using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StackSearcher : MonoBehaviour
{
    [SerializeField] private Stack stack;
    [SerializeField] private LayerMask searchMask;
    [SerializeField] private float searchRadius;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

    private void Awake()
    {
        StartCoroutine(Search());
    }
    
    private IEnumerator Search()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            var foundObjects = Physics.OverlapSphere(transform.position, searchRadius, searchMask).ToList();
            var sortFoundObjects = new List<StackableItem>();
            foreach (var stackable in foundObjects)
            {
                var stackableItem = stackable.GetComponent<StackableItem>();
                if (stackableItem && stackableItem.IsAttach == false && stackableItem.IsStackableItemActivate)
                {
                    sortFoundObjects.Add(stackableItem);
                }
            }

            foreach (var sortFoundObject in sortFoundObjects)
            {
                if (stack.IsCanBeAttach(sortFoundObject))
                {
                    stack.Attach(sortFoundObject);
                    break;
                }
            }
        }
    }
}