using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EventBusSystem;
using Kuhpik;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stack : MonoBehaviour, IRecalculateAllShopPanelSignal
{
    [SerializeField] private Canvas stackCanvas;
    [SerializeField] private TMP_Text canvasStackSizeText;
    [SerializeField] private Transform firstPositions;
    [SerializeField] private Vector3 addNextPoint;
    [SerializeField] private Vector3 addNextPointChild;

    private List<StackableItem> m_stackableItems = new List<StackableItem>();
    private GameData m_gameData;

    public bool PauseStack;

    private bool isStackEnable = true;
    private int currentMaxAttach => Bootstrap.Instance.PlayerData.UpgadeLevel[UpgradeType.Stack];

    public bool IsCanBeAttach(StackableItem stackableItem)
    {
        if (m_stackableItems.Count == 0)
        {
            return true;
        }

        if (stackableItem.StackableType != m_stackableItems[0].StackableType) return true;
        return m_stackableItems.Count < currentMaxAttach;
    }

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public Transform DetachLast()
    {
        if (m_stackableItems.Count == 0 || m_stackableItems[0].StackableType == StackableType.Child) return null;
        var prop = m_stackableItems.Last();
        m_stackableItems.Remove(prop);
        prop.Detach();
        prop.IsAttach = true;
        AnimationRecalculate();
        return prop.transform;
    }

    public void Attach(StackableItem stackableItem)
    {
        if (isStackEnable == false || PauseStack) return;
        if (stackableItem.StackableType == StackableType.Child)
        {
            stackableItem.GetComponent<Child>().ChildAnimationController.SetBool(ChildAnimationType.InHands, true);
        }

        if (m_stackableItems.Count != 0 && m_stackableItems[0].StackableType != stackableItem.StackableType)
        {
            DropAllAttach();
        }

        m_stackableItems.Add(stackableItem);
        if (stackableItem.StackableType == StackableType.Child)
        {
            var childPos = addNextPointChild;
            childPos.y = 0;
            if (m_stackableItems.Count != 1)
            {
                childPos += new Vector3(0, addNextPointChild.y, 0) * (m_stackableItems.Count - 1);
            }

            stackableItem.JumpToStack(firstPositions, childPos);
        }
        else
        {
            stackableItem.JumpToStack(firstPositions, addNextPoint * m_stackableItems.Count);
        }

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
                prop.IsAttach = true;
                prop.transform.DOScale(Vector3.zero, 1.5f).OnComplete(() => Destroy(prop.gameObject));
                prop.transform.DOJump(
                    new Vector3(transform.position.x + Random.Range(-2, 2), 1.475f,
                        transform.position.z + Random.Range(-2, 2)), 1f, 1, 0.55f);
            }

            m_stackableItems.Remove(stackableItem);
            stackableItem.AttackPause();
            DropAllAttach();
            AnimationRecalculate();
            break;
        }
    }

    public void DisableStack()
    {
        isStackEnable = false;
        m_gameData.CharacterAnimationController.SetBool(CharacterAnimationType.ItemSelect, false);
        firstPositions.gameObject.SetActive(false);
    }

    public void EnableStack()
    {
        isStackEnable = true;
        firstPositions.gameObject.SetActive(true);
        RecalculateChild();
        AnimationRecalculate();
    }


    public bool IsCanBeDetachProp(PropType propType)
    {
        if (m_stackableItems.Count == 0 ||
            m_stackableItems[0].StackableType != StackableType.Prop ||
            m_stackableItems.Count(t => t.GetComponent<Prop>().PropType == propType) ==
            0) return false;
        return true;
    }

    public bool IsCanBeDetachEatChild(bool isSecond)
    {
        if (m_stackableItems.Count == 0 ||
            m_stackableItems[0].StackableType != StackableType.Child ||
            m_stackableItems.Count(t =>
            {
                var child = t.GetComponent<Child>();
                return child.CurrentChildType == ChildStateType.Eat && child.IsSecondChild == isSecond;
            }) ==
            0) return false;
        return true;
    }

    public bool IsCanBeDetachSleepChild()
    {
        if (m_stackableItems.Count == 0 ||
            m_stackableItems[0].StackableType != StackableType.Child ||
            m_stackableItems.Count(t =>
            {
                var child = t.GetComponent<Child>();
                return child.CurrentChildType == ChildStateType.Sleep;
            }) ==
            0) return false;
        return true;
    }

    public bool IsCanBeDetachMotherChild(bool isSecond)
    {
        if (m_stackableItems.Count == 0 ||
            m_stackableItems[0].StackableType != StackableType.Child ||
            m_stackableItems.Count(t =>
            {
                var child = t.GetComponent<Child>();
                return child.CurrentChildType == ChildStateType.ToMother && child.IsSecondChild == isSecond;
            }) ==
            0) return false;
        return true;
    }

    public Prop DetachProp(PropType propType)
    {
        foreach (var stackableItem in m_stackableItems)
        {
            var prop = stackableItem.GetComponent<Prop>();
            if (prop && prop.PropType == propType)
            {
                m_stackableItems.Remove(stackableItem);
                RecalculateChild();
                stackableItem.Detach();
                AnimationRecalculate();
                return prop;
            }
        }

        AnimationRecalculate();
        return null;
    }

    public Child DetachChild(ChildStateType childStateType)
    {
        foreach (var stackableItem in m_stackableItems)
        {
            var child = stackableItem.GetComponent<Child>();
            if (child && child.CurrentChildType == childStateType)
            {
                m_stackableItems.Remove(stackableItem);
                RecalculateChild();
                child.ChildAnimationController.SetBool(ChildAnimationType.InHands, false);
                stackableItem.Detach();
                AnimationRecalculate();
                return child;
            }
        }

        AnimationRecalculate();
        return null;
    }

    private void RecalculateChild()
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

    private void AnimationRecalculate()
    {
        m_gameData ??= Bootstrap.Instance.GameData;
        canvasStackSizeText.text = $"{m_stackableItems.Count}/{currentMaxAttach}";
        stackCanvas.enabled = m_stackableItems.Count != 0;
        m_gameData.CharacterAnimationController.SetBool(CharacterAnimationType.ItemSelect, m_stackableItems.Count != 0);
    }

    public void Recalculate()
    {
        canvasStackSizeText.text = $"{m_stackableItems.Count}/{currentMaxAttach}";
    }
}