using System;
using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class CharacterDiaperSubstitute : MonoBehaviour
{
    [SerializeField] private Stack stack;
    private CharacterControllerSystem controllerSystem;

    private void OnTriggerEnter(Collider other)
    {
        var child = other.GetComponent<Child>();
        if (child && child.CurrentChildType == ChildStateType.Poop && stack.IsCanBeDetachProp(PropType.DiaperProp))
        {
            controllerSystem ??= Bootstrap.Instance.GetSystem<CharacterControllerSystem>();
            controllerSystem.TryToChangeChildDiaper(child, stack);
            Destroy(stack.DetachProp(PropType.DiaperProp).gameObject);
            stack.DisableStack();
        }
    }
}