using System.Collections;
using UnityEngine;

namespace EventBusSystem
{
    public interface IUpdateMoney : IGlobalSubscriber
    {
        void UpdateMoney();
    }

    public interface IRecalculateAllShopPanelSignal : IGlobalSubscriber
    {
        void Recalculate();
    }
}