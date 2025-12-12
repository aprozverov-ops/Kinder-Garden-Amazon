using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kuhpik;
using UnityEngine;

public class PointerActivator : GameSystem
{
    [SerializeField] private List<MapArrowPointer> arrowPointers;
    [SerializeField] private List<MapArrowPointer> arrowPointersSecond;
    [SerializeField] private List<PointerConfiguration> pointerConfigurations;

    public override void OnInit()
    {
        UpdatePointers();
    }

    public MapArrowPointer SetPointer(PointerType pointerType, Transform target, bool isSecondRoom)
    {
        var foundPointer = isSecondRoom
            ? arrowPointersSecond.Where(t => t.IsPointActivate).ToList()
            : arrowPointers.Where(t => t.IsPointActivate).ToList();
        if (foundPointer.Count == 0) return null;

        var pointerConfiguration = pointerConfigurations.Where(t => t.PointerType == pointerType).ToList()[0];
        foundPointer[0].SetTarget(target, pointerConfiguration.Sprite);
        return foundPointer[0];
    }

    public void UpdatePointers()
    {
        if (game.isSecondRoom)
        {
            foreach (var mapArrowPointer in arrowPointersSecond)
            {
                mapArrowPointer.IsActivateRoom = true;
            }
            foreach (var mapArrowPointer in arrowPointers)
            {
                mapArrowPointer.IsActivateRoom = false;
            }
        }
        else
        {
            foreach (var mapArrowPointer in arrowPointers)
            {
                mapArrowPointer.IsActivateRoom = true;
            }
            foreach (var mapArrowPointer in arrowPointersSecond)
            {
                mapArrowPointer.IsActivateRoom = false;
            }
        }
    }
}