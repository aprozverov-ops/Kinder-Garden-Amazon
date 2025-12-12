using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChildWantSleepPlace : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Transform childPosition;

    private Child currentStack;

    public bool IsPlaceFree => currentStack == null;
    public void DisableChild()
    {
        currentStack = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        var stack = other.GetComponent<Stack>();
        if (stack && (stack.IsCanBeDetachSleepChild()))
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

    public void Activate(Child child)
    {
        currentStack = child;
        currentStack.TryToSleep(this);
        currentStack.transform.DOJump(childPosition.position, 1, 1, 0.4f);
        currentStack.transform.DORotateQuaternion(childPosition.rotation, 0.2f);
        currentStack.ChildOnSleepPlace = true;
    }

    private void Activate(Stack stack)
    {
        
        if (stack.IsCanBeDetachSleepChild())
        {
            if (currentStack != null)
            {
                if (currentStack.CurrentChildType != ChildStateType.ToMother) return;
                stack.Attach(currentStack.GetComponent<StackableItem>());
            }
            VibrationSystem.Play();
            var newChild = stack.DetachChild(ChildStateType.Sleep);
            newChild.ChildOnSleepPlace = true;
            currentStack = newChild;
            currentStack.TryToSleep(this);
            newChild.transform.DOJump(childPosition.position, 1, 1, 0.4f);
            newChild.transform.DORotateQuaternion(childPosition.rotation, 0.2f);
        }
    }
}