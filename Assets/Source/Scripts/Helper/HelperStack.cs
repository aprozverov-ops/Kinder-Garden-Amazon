using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class HelperStack : MonoBehaviour
{
    [SerializeField] private Helper helper;
    [SerializeField] private Transform firstPositions;
    [SerializeField] private Vector3 addNextPoint;
    [SerializeField] private Vector3 addNextPointChild;

    private List<StackableItem> m_stackableItems = new List<StackableItem>();

    public void Attach(StackableItem stackableItem)
    {
        if (stackableItem.StackableType == StackableType.Child)
        {
            stackableItem.GetComponent<Child>().ChildAnimationController.SetBool(ChildAnimationType.InHands, true);
        }

        m_stackableItems.Add(stackableItem);
        stackableItem.JumpToStack(firstPositions,
            stackableItem.StackableType == StackableType.Child
                ? addNextPointChild * m_stackableItems.Count
                : addNextPoint * m_stackableItems.Count);
        AnimationRecalculate();
    }

    public void DropAllAttach()
    {
        foreach (var stackableItem in m_stackableItems)
        {
            stackableItem.Detach();
            var child = stackableItem.GetComponent<Child>();
            if (child)
            {
                child.OnDrop();
            }

            var prop = stackableItem.GetComponent<Prop>();
            if (prop)
            {
                prop.Drop();
                prop.transform.DOScale(Vector3.zero, 0.4f).OnComplete(() => Destroy(prop.gameObject));
            }

            m_stackableItems.Remove(stackableItem);
            stackableItem.AttackPause();
            AnimationRecalculate();
            DropAllAttach();
            break;
        }
    }

    public Child DetachChild(ChildStateType childStateType)
    {
        foreach (var stackableItem in m_stackableItems)
        {
            var child = stackableItem.GetComponent<Child>();
            if (child && child.CurrentChildType == childStateType)
            {
                m_stackableItems.Remove(stackableItem);
                Recalculate();
                child.ChildAnimationController.SetBool(ChildAnimationType.InHands, false);
                stackableItem.Detach();
                AnimationRecalculate();
                return child;
            }
        }

        AnimationRecalculate();
        return null;
    }

    public Prop DetachProp(PropType propType)
    {
        foreach (var stackableItem in m_stackableItems)
        {
            var prop = stackableItem.GetComponent<Prop>();
            if (prop && prop.PropType == propType)
            {
                m_stackableItems.Remove(stackableItem);
                Recalculate();
                stackableItem.Detach();
                AnimationRecalculate();
                return prop;
            }
        }

        AnimationRecalculate();
        return null;
    }

    public bool IsCanBeDetachProp(PropType propType)
    {
        if (m_stackableItems.Count == 0 ||
            m_stackableItems[0].StackableType != StackableType.Prop ||
            m_stackableItems.Count(t => t.GetComponent<Prop>().PropType == propType) ==
            0) return false;
        return true;
    }

    private void AnimationRecalculate()
    {
        helper.HelperAnimatorController.SetBool(CharacterAnimationType.ItemSelect, m_stackableItems.Count != 0);
    }

    private void Recalculate()
    {
        var childs = m_stackableItems.ToList();
        m_stackableItems = new List<StackableItem>();
        foreach (var child in childs)
        {
            m_stackableItems.Add(child);
            child.TeleportToStack(firstPositions,
                m_stackableItems[0].StackableType == StackableType.Child
                    ? addNextPointChild * m_stackableItems.Count
                    : addNextPoint * m_stackableItems.Count);
        }
    }
}