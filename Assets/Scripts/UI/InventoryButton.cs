using System;
using System.Collections;
using System.Collections.Generic;
using SIM.Core;
using UnityEngine;

namespace SIM.UI
{
    public class InventoryButton : MonoBehaviour
    {
        public Action<InventoryButton> onClicked;

        public Item Item;

        public void OnClicked()
        {
            onClicked?.Invoke(this);
        }
    }
}
