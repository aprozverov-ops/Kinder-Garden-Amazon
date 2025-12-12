using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ActivatePanelController : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    private void OnEnable()
    {
        transform.position = startPos.position;
        transform.DOMove(endPos.position, 0.3f);
    }
}