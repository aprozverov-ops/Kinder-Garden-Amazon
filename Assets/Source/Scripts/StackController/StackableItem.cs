using System.Collections;
using DG.Tweening;
using UnityEngine;

public class StackableItem : MonoBehaviour
{
    [SerializeField] private Vector3 localRotate;
    [SerializeField] private float powerJump = 3;
    [SerializeField] private StackableType stackableType;

    public bool IsAttach { get;  set; }
    public bool IsStackableItemActivate = true;
    public StackableType StackableType => stackableType;

    public virtual void JumpToStack(Transform parent, Vector3 localPos)
    {
        IsAttach = true;
        transform.parent = parent;
        transform.DOLocalJump(localPos, powerJump, 1, 0.4f).OnComplete(() =>
        {
            transform.DOLocalRotate(localRotate, 0.2f);
        });
        VibrationSystem.Play();
    }

    public void TeleportToStack(Transform parent, Vector3 localPos)
    {
        IsAttach = true;
        transform.parent = parent;
        transform.DOLocalJump(localPos, powerJump, 1, 0.1f).OnComplete(() =>
        {
            transform.DOLocalRotate(localRotate, 0f);
        });
    }

    public void AttackPause()
    {
        StartCoroutine(AttackPauseC());
    }

    private IEnumerator AttackPauseC()
    {
        IsAttach = true;
        yield return new WaitForSeconds(2f);
        IsAttach = false;
    }

    public void Detach()
    {
        transform.parent = null;
        IsAttach = false;
    }
}