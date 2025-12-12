using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildGiver : MonoBehaviour
{
    [SerializeField] private Stack stack;

    private void OnTriggerEnter(Collider other)
    {
        var mother = other.GetComponent<Mother>();
        if (mother && mother.Child == null && stack.IsCanBeDetachMotherChild(mother
            .IsSecond))
        {
            var detachChild = stack.DetachChild(ChildStateType.ToMother);
            mother.TakeChild(detachChild,detachChild.IsSecondChild);
        }
    }
}