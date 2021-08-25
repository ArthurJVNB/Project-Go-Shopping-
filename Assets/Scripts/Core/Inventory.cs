using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIM.Core
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] float money;
        [SerializeField] List<GameObject> items = new List<GameObject>();

        public void Add(GameObject item)
        {
            items.Add(item);
        }

        public bool Remove(GameObject item)
        {
            return items.Remove(item);
        }
    }
}
