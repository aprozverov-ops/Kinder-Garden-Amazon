using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    public static TutorialArrow Instance;
    [SerializeField] private float forwardSetting;
    [SerializeField] private float upSetting;
    [SerializeField] private Transform car;
    [SerializeField] private Transform arrow;
    [SerializeField] private float indificationDistance;

    [SerializeField] private Transform SetTargetMoment;
    private Transform target;
    private Animator animator;
    private bool isInIndification;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Awake()
    {
        Instance = this;
        animator = arrow.GetComponent<Animator>();
        DisableArrow();
    }

    private void Update()
    {
        if (SetTargetMoment != null)
        {
            SetTarget(SetTargetMoment);
            SetTargetMoment = null;
            ShowArrow();
        }

        if (target != null)
        {
            if (Vector3.Distance(car.transform.position, target.position) < indificationDistance)
            {
                ToIndification();
            }
            else
            {
                RotateToObject();
            }
        }
    }

    public void ShowArrow()
    {
        arrow.DOKill();
        arrow.DOScale(Vector3.one, 0.3f);
        StopAllCoroutines();
        var playerPosition = car.position;
        playerPosition.y = 0;
        var targerPos = target.position;
        targerPos.y = 0;
        var direction = targerPos - playerPosition;
        Quaternion arrowRotation = Quaternion.LookRotation(direction);

        var pos = playerPosition + Vector3.up * upSetting;
        pos = pos + arrow.forward * forwardSetting;
        arrow.position = pos;
        arrow.rotation = arrowRotation;
    }

    public void DisableArrow()
    {
        isInIndification = false;
        //arrow.transform.position = new Vector3(0, -125, 0);
        StopAllCoroutines();
        StartCoroutine(Disable());
        animator.SetBool("Play", false);
    }

    private IEnumerator Disable()
    {
        arrow.DOKill();
        arrow.DOScale(Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.5f);
    }

    public void RotateToObject()
    {
        if (isInIndification)
        {
            isInIndification = false;
            animator.SetBool("Play", false);
        }

        isInIndification = false;
        var playerPosition = car.position;
        playerPosition.y = 0;
        var targerPos = target.position;
        targerPos.y = 0;
        var direction = targerPos - playerPosition;
        Quaternion arrowRotation = Quaternion.LookRotation(direction);

        var pos = playerPosition + Vector3.up * upSetting;
        pos = pos + arrow.forward * forwardSetting;
        arrow.position = Vector3.Lerp(arrow.position, pos, 6 * Time.deltaTime);
        arrow.rotation =
            Quaternion.Lerp(arrow.rotation, arrowRotation, 6 * Time.deltaTime);
    }

    public void ToIndification()
    {
        if (isInIndification)
        {
            return;
        }

        animator.SetBool("Play", true);
        isInIndification = true;
        arrow.DOMove(target.position, 0.5f);
    }
}