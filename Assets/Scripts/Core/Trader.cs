using UnityEngine;

namespace SIM.Core
{
    [RequireComponent(typeof(Inventory))]
    public class Trader : MonoBehaviour
    {
        public Inventory Inventory { get; private set; }

        private void Awake()
        {
            Inventory = GetComponent<Inventory>();
            // UpdateOwnership(); // inventory already calls onInventoryChanged on Start().
        }

        private void OnEnable() => Inventory.onInventoryChanged += UpdateOwnership;

        private void OnDisable() => Inventory.onInventoryChanged -= UpdateOwnership;

        public bool Trade(Item item, Trader to)
        {
            bool result = false;

            if (Inventory.Contains(item))
            {
                if (to.Inventory.SubtractMoney(item.Price))
                {
                    Inventory.AddMoney(item.Price);
                    Inventory.Remove(item);
                    to.Inventory.Add(item);
                    result = true;
                }
            }

            return result;
        }

        private void UpdateOwnership()
        {
            Item[] items = Inventory.GetItems();
            foreach (Item item in items)
            {
                item.SetOwner(this);
            }
        }
    }
}