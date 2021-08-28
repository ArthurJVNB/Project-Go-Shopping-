using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIM.Core
{
    public class Inventory : MonoBehaviour
    {
        public Action onInventoryChanged;
        public Action<Item> onInventoryAddedItem;
        public Action<Item> onInventoryRemovedItem;

        public float Money { get { return moneyAmount; } }

        [SerializeField] float moneyAmount;
        [SerializeField] List<Item> items = new List<Item>();

        private void Start()
        {
            onInventoryChanged?.Invoke();
        }

        public void Add(Item item)
        {
            item.onChangedState += OnItemChangedState;

            items.Add(item);

            onInventoryAddedItem?.Invoke(item);
            onInventoryChanged?.Invoke();
        }

        public bool Remove(Item item)
        {
            item.onChangedState -= OnItemChangedState;

            bool result = items.Remove(item);

            onInventoryRemovedItem?.Invoke(item);
            onInventoryChanged?.Invoke();

            return result;
        }

        public void AddMoney(float value)
        {
            moneyAmount += value;
        }

        public bool SubtractMoney(float value)
        {
            if (moneyAmount - value < 0) return false;

            moneyAmount -= value;
            return true;
        }

        public bool Contains(Item item)
        {
            return items.Contains(item);
        }

        public Item[] GetItems()
        {
            return items.ToArray();
        }

        public Item[] GetItems(Item.State[] desiredStates)
        {
            List<Item> filteredItems = new List<Item>();
            foreach (Item.State desiredState in desiredStates)
            {
                foreach (Item item in items)
                {
                    if (item.CurrentState == desiredState) filteredItems.Add(item);
                }
            }
            return filteredItems.ToArray();
        }

        private void OnItemChangedState(Item item)
        {
            Invoke("InvokeDelayed", 0.1f);
        }

        private void InvokeDelayed()
        {
            onInventoryChanged?.Invoke();
        }
    }
}
