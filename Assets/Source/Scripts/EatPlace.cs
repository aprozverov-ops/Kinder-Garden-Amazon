using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EatPlace : MonoBehaviour
{
    [SerializeField] private bool isSecond;
    [SerializeField] private Image image;
    [SerializeField] private Transform childPosition;

    private Child currentStack;

    public bool IsSecond
    {
        get => isSecond;
        set => isSecond = value;
    }

    public bool IsPlaceFree => currentStack == null;

    public void DisableChild()
    {
        currentStack = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        var stack = other.GetComponent<Stack>();
        if (stack && (stack.IsCanBeDetachEatChild(isSecond) || stack.IsCanBeDetachProp(PropType.Bottle) ||
                      stack.IsCanBeDetachProp(PropType.Kasha)))
        {
            image.DOKill();
            image.DOFillAmount(1, 0.7f).OnComplete(() => Activate(stack));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var stack = other.GetComponent<Stack>();
        if (stack)
        {
            image.DOKill();
            image.DOFillAmount(0, 0.4f);
        }
    }

    public void ActivateChild(Child child)
    {
        currentStack = child;
        currentStack.TryToEat(true, this);
        child.transform.DOJump(childPosition.position, 1, 1, 0.4f);
        child.transform.DORotateQuaternion(childPosition.rotation, 0.2f);
    }

    private void Activate(Stack stack)
    {
        if (stack.IsCanBeDetachEatChild(isSecond))
        {
            if (currentStack != null)
            {
                if (currentStack.CurrentChildType == ChildStateType.ToMother)
                {
                    stack.Attach(currentStack.GetComponent<StackableItem>());
                    currentStack.CurrentEatPlace = null;
                }
                else return;
            }

            VibrationSystem.Play();
            var newChild = stack.DetachChild(ChildStateType.Eat);
            currentStack = newChild;
            newChild.ChildOnEatPlace = true;
            currentStack.TryToEat(true, this);
            newChild.transform.DOJump(childPosition.position, 1, 1, 0.4f);
            newChild.transform.DORotateQuaternion(childPosition.rotation, 0.2f);
        }
        else
        {
            if (isSecond == false)
            {
                if (stack.IsCanBeDetachProp(PropType.Bottle) && currentStack != null &&
                    currentStack.CurrentChildType == ChildStateType.Eat)
                {
                    VibrationSystem.Play();
                    var bottle = stack.DetachProp(PropType.Bottle);
                    bottle.IsAttach = true;
                    bottle.IsStackableItemActivate = false;
                    bottle.transform.DOJump(childPosition.position, 1, 1, 0.3f).OnComplete(() =>
                    {
                        currentStack.TryToEat(false, this);
                        Destroy(bottle.gameObject);
                    });
                    bottle.transform.DORotateQuaternion(childPosition.rotation, 0.3f);
                }
            }
            else
            {
                if (stack.IsCanBeDetachProp(PropType.Kasha) && currentStack != null &&
                    currentStack.CurrentChildType == ChildStateType.Eat)
                {
                    VibrationSystem.Play();
                    var bottle = stack.DetachProp(PropType.Kasha);
                    bottle.IsAttach = true;
                    bottle.IsStackableItemActivate = false;
                    bottle.transform.DOJump(childPosition.position, 1, 1, 0.3f).OnComplete(() =>
                    {
                        currentStack.TryToEat(false, this);
                        Destroy(bottle.gameObject);
                    });
                    bottle.transform.DORotateQuaternion(childPosition.rotation, 0.3f);
                }
            }
        }
    }
}