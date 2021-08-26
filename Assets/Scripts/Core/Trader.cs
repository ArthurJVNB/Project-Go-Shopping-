using UnityEngine;

namespace SIM.Core
{
    [RequireComponent(typeof(Inventory))]
    public class Trader : MonoBehaviour
    {
        Inventory inventory;


        private void Start()
        {
            inventory = GetComponent<Inventory>();
            UpdateOwnership();
        }

        private void OnEnable() => inventory.onInventoryChanged += UpdateOwnership;

        private void OnDisable() => inventory.onInventoryChanged -= UpdateOwnership;

        public bool Trade(Item item, Trader to)
        {
           
        }

        private void UpdateOwnership()
        {
            Item[] items = inventory.GetItems();
            foreach (Item item in items)
            {
                item.SetOwner(this);
            }
        }
    }
}