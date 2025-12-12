using UnityEngine;

public class Prop : StackableItem
{
    [SerializeField] private PropType propType;

    public PropType PropType => propType;
    
    public void Drop()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public override void JumpToStack(Transform parent, Vector3 localPos)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        base.JumpToStack(parent, localPos);
    }
}