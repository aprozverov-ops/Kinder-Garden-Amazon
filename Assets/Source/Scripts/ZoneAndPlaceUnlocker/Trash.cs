using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Trash : MonoBehaviour
{
    [SerializeField] private GameObject trashPosition;
    [SerializeField] private Image image;

    private void OnTriggerEnter(Collider other)
    {
        var stack = other.GetComponent<Stack>();
        if (stack)
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


    private void Activate(Stack stack)
    {
        var last = stack.DetachLast();
        if (last != null)
        {
            last.DOJump(trashPosition.transform.position, 1, 1, 0.35f).OnComplete(() => Destroy(last.gameObject));
            last.DORotateQuaternion(Random.rotation, 0.3f);
            image.DOKill();
            image.DOFillAmount(0, 0.4f).OnComplete(() =>
            {
                image.DOFillAmount(1, 0.7f).OnComplete(() => Activate(stack));
            });
        }
    }
}