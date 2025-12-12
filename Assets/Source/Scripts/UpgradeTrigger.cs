using DG.Tweening;
using Kuhpik;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    [SerializeField] private Image image;

    private bool isInTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (isInTrigger) return;
        var stack = other.GetComponent<Stack>();
        if (stack)
        {
            isInTrigger = true;
            image.DOKill();
            image.DOFillAmount(1, 0.7f).OnComplete(Activate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var stack = other.GetComponent<Stack>();
        if (stack && player.isKinematic==false)
        {
            image.DOKill();
            image.DOFillAmount(0, 0.4f).OnComplete(() => isInTrigger = false);
        }
    }

    private void Activate()
    {
        image.DOKill();
        player.isKinematic = true;
        player.transform.DORotate(new Vector3(
            0, 180, 0), 0.4f);
        Bootstrap.Instance.ChangeGameState(GameStateID.Menu);
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Upgrade);
    }

    public void InGame()
    {
        player.isKinematic = false;
        Bootstrap.Instance.ChangeGameState(GameStateID.Game);
        Bootstrap.Instance.GetSystem<CameraController>().ChangeCamera(CameraType.Walk);
    }
}