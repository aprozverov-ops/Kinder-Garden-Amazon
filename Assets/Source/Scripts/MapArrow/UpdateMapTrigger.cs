using Kuhpik;
using UnityEngine;

public class UpdateMapTrigger : MonoBehaviour
{
    [SerializeField] private bool isSecond;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Stack>())
        {
            Bootstrap.Instance.GameData.isSecondRoom = isSecond;
            Bootstrap.Instance.GetSystem<PointerActivator>().UpdatePointers();
        }
    }
}