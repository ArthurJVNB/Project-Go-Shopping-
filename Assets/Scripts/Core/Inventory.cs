using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIM.Core
{
    public class Inventory : MonoBehaviour
    {
        public Action onInventoryChanged;
        public float Money { get { return moneyAmount; } }

        [SerializeField] float moneyAmount;
        [SerializeField] List<Item> items = new List<Item>();

        private void Start()
        {
            onInventoryChanged?.Invoke();
        }

        public void Add(Item item)
        {
            items.Add(item);
            onInventoryChanged?.Invoke();
        }

        public bool Remove(Item item)
        {
            bool result = items.Remove(item);
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
    }
}
