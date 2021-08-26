using UnityEngine;
using SIM.Core;
using SIM.UI;
using UnityEngine.UI;

namespace SIM.Control
{
    [RequireComponent(typeof(Trader))]
    public class ShopkeeperControl : MonoBehaviour, IInteractable
    {
        [SerializeField] InventoryUI inventoryUI;

        Trader trader;

        private void Awake()
        {
            trader = GetComponent<Trader>();
        }

        private void OnEnable() => inventoryUI.onItemClicked += SellToPlayer;

        private void OnDisable() => inventoryUI.onItemClicked -= SellToPlayer;

        public void Interact(GameObject whoInteracts, out GameObject interacted)
        {
            interacted = gameObject;
            print("<INVENTORY APPEARS> I supply only the finest goods");

            if (whoInteracts.CompareTag("Player"))
            {
                Trader player = whoInteracts.GetComponent<Trader>();
                Item itemFromPlayer = player.Inventory.GetItems()[0];
                if (itemFromPlayer)
                {
                    player.Trade(itemFromPlayer, this.trader);
                }
            }
        }

        private void SellToPlayer(Item item)
        {
            Trader player = GameObject.FindWithTag("Player").GetComponent<Trader>();
            trader.Trade(item, player);
        }
    }
}