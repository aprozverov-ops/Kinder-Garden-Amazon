using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class
    ChildSpriteActivator : MonoBehaviour
{
    private ChildSpriteRotator spriteRotator;
    [SerializeField] private StackableItem stackableItem;
    [SerializeField] private List<ChildSpriteConfiguration> childSpriteConfigurations;
    [SerializeField] private Image image;
    [SerializeField] private Image imageIcon;

    private bool isActivate;

    private void Awake()
    {
        spriteRotator = GetComponent<ChildSpriteRotator>();
        DisableImage();
    }

    private void Update()
    {
        image.enabled = !stackableItem.IsAttach && isActivate && spriteRotator.IsEnabe == false;
        imageIcon.enabled = !stackableItem.IsAttach && isActivate;
    }

    public void ActivateSprite(ChildStateType spriteType)
    {
        isActivate = true;
        imageIcon.enabled = true;
        foreach (var childSpriteConfiguration in childSpriteConfigurations)
        {
            if (childSpriteConfiguration.SpriteType == spriteType)
            {
                imageIcon.sprite = childSpriteConfiguration.Sprite;
            }
        }
    }

    public void DisableImage()
    {
        isActivate = false;
        image.enabled = false;
    }
}