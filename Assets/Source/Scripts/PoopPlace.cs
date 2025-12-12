using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PoopPlace : MonoBehaviour
{
    
    //legacy
    // [SerializeField] private Image image;
    // [SerializeField] private Transform childPosition;
    //
    // private Child currentStack;
    //
    // private void OnTriggerEnter(Collider other)
    // {
    //     var stack = other.GetComponent<Stack>();
    //     if (stack && stack.IsCanBeDetachPoopChild())
    //     {
    //         image.DOKill();
    //         image.DOFillAmount(1, 0.7f).OnComplete(() => Activate(stack));
    //     }
    // }
    //
    // private void OnTriggerExit(Collider other)
    // {
    //     var stack = other.GetComponent<Stack>();
    //     if (stack)
    //     {
    //         image.DOKill();
    //         image.DOFillAmount(0, 0.4f);
    //     }
    // }
    //
    //
    // private void Activate(Stack stack)
    // {
    //     if (currentStack == null && stack.IsCanBeDetachPoopChild())
    //     {
    //         var newChild = stack.DetachChild(ChildStateType.Poop);
    //         currentStack = newChild;
    //         newChild.TryToPoop();
    //         newChild.transform.position = childPosition.position;
    //         newChild.transform.rotation = childPosition.rotation;
    //     }
    // }
}