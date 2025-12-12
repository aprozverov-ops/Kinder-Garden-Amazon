using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MapArrowPointer : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float borderSize;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Transform mapPosition;
    private RectTransform pointerRectTransform;
    private bool isEnable;
    public bool IsPointActivate => mapPosition == null;

    public bool IsActivateRoom;
    private void Awake()
    {
        pointerRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        this.image.enabled = mapPosition != null && IsActivateRoom;
        if (mapPosition != null)
        {
            Move();
        }
    }

    public void SetTarget(Transform target, Sprite image)
    {
        if (target == null)
        {
            mapPosition = target;
        }
        else
        {
            mapPosition = target;
            var targetPositionScreen = Camera.main.WorldToScreenPoint(mapPosition.position);
            var isOffScreen = targetPositionScreen.x <= borderSize ||
                              targetPositionScreen.x >= Screen.width - borderSize ||
                              targetPositionScreen.y <= borderSize ||
                              targetPositionScreen.y >= Screen.height - borderSize;
            isEnable = !isOffScreen;
            mapPosition = target;
            this.image.sprite = image;
        }
    }

    public void Move()
    {
        var toPos = mapPosition.position;
        var fromPos = Camera.main.transform.position;
        fromPos.z = 0;
        var dir = (toPos - fromPos).normalized;
        var floatAngle = GetAngleFromVectorFloat(dir);
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, floatAngle);

        var targetPositionScreen = Camera.main.WorldToScreenPoint(mapPosition.position);
        var isOffScreen = targetPositionScreen.x <= borderSize || targetPositionScreen.x >= Screen.width - borderSize ||
                          targetPositionScreen.y <= borderSize || targetPositionScreen.y >= Screen.height - borderSize;

        if (isOffScreen)
        {
            if (isEnable == false)
            {
                isEnable = true;
                image.transform.DOKill();
                image.transform.DOScale(Vector3.one, 0.5f);
            }

            var cappedPosition = targetPositionScreen;
            if (cappedPosition.x <= borderSize) cappedPosition.x = borderSize;
            if (cappedPosition.x >= Screen.width - borderSize) cappedPosition.x = Screen.width - borderSize;

            if (cappedPosition.y <= borderSize) cappedPosition.y = borderSize;
            if (cappedPosition.y >= Screen.height - borderSize) cappedPosition.y = Screen.height - borderSize;

            var pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedPosition);
            pointerRectTransform.position = pointerWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x,
                pointerRectTransform.localPosition.y, 0f);
        }
        else
        {
            if (isEnable)
            {
                isEnable = false;
                image.transform.DOKill();
                image.transform.DOScale(Vector3.zero, 0.5f);
            }

            var pointerWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreen);
            pointerRectTransform.position = pointerWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x,
                pointerRectTransform.localPosition.y, 0);
        }
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}