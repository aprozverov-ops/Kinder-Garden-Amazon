using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventBusSystem;
using Kuhpik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceUnlocker : MonoBehaviour
{
    [SerializeField] private bool spawnEffectOnFirstElement;
    [SerializeField] private string nameTutorial;
    [SerializeField] private Animator m_animator;
    [SerializeField] private TMP_Text price;
    [SerializeField] private Image image;
    [SerializeField] private List<GameObject> place;
    [SerializeField] private PlaceUnlockPriceConfiguration placeUnlockPriceConfiguration;
    [SerializeField] private int id;

    private IEnumerator Initialize()
    {
        price.text = placeUnlockPriceConfiguration.Price.ToString();
        yield return new WaitForSeconds(0.3f);
        if (Bootstrap.Instance.PlayerData.boughtPlaces.Contains(id))
        {
            if (m_animator != null)
                m_animator.SetBool("Play", true);
            Destroy(gameObject);
            foreach (var place in place)
            {
                place.SetActive(true);
            }
        }
        else
        {
            foreach (var place in place)
            {
                var placeUnlocker = place.GetComponent<PlaceUnlocker>();
                if (placeUnlocker)
                {
                    placeUnlocker.StartDisable();
                }
                else
                    place.SetActive(false);
            }
        }
    }

    public void StartDisable()
    {
        StartCoroutine(DisablePlace());
    }

    private IEnumerator DisablePlace()
    {
        yield return null;
        gameObject.SetActive(false);
        foreach (var place in place)
        {
            place.SetActive(false);
        }
    }

    private void Awake()
    {
        StartCoroutine(Initialize());
    }

    private void OnTriggerEnter(Collider other)
    {
        var stack = other.GetComponent<Stack>();
        if (stack)
        {
            image.DOKill();
            image.DOFillAmount(1, 0.7f).OnComplete(() => Activate());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var stack = other.GetComponent<Stack>();
        if (stack)
        {
            image.DOKill();
            image.DOFillAmount(0, 0.4f);
        }
    }


    private void Activate()
    {
        if (Bootstrap.Instance.PlayerData.Money >= placeUnlockPriceConfiguration.Price)
        {
            Bootstrap.Instance.PlayerData.Money -= placeUnlockPriceConfiguration.Price;
            Bootstrap.Instance.PlayerData.boughtPlaces.Add(id);
            Bootstrap.Instance.SaveGame();
            foreach (var place in place)
            {
                place.SetActive(true);
                var scale = place.transform.localScale;
                place.transform.localScale = Vector3.zero;
                place.transform.DOScale(scale, 0.4f);
            }

            if (spawnEffectOnFirstElement)
            {
                Bootstrap.Instance.GetSystem<EffectSpawner>().SpawnEffect(EffectType.Confity, place[0].transform);
            }

            Destroy(gameObject);
            if (m_animator != null)
                m_animator.SetBool("Play", true);
            EventBus.RaiseEvent<IUpdateMoney>(t => t.UpdateMoney());
            VibrationSystem.Play();
        }
    }
}