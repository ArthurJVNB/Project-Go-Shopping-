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
        [SerializeField] float rangeToContinueTrade = 2f;

        const string PLAYER_TAG = "Player";
        Trader trader;

        private void Awake()
        {
            trader = GetComponent<Trader>();
        }

        private void OnEnable() => inventoryUI.onItemClicked += SellToPlayer;

        private void OnDisable() => inventoryUI.onItemClicked -= SellToPlayer;

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag(PLAYER_TAG))
            {
                inventoryUI.HideUI();
            }
        }

        public void Interact(GameObject whoInteracts, out GameObject interacted)
        {
            interacted = gameObject;
            print("<INVENTORY APPEARS> I supply only the finest goods");

            if (whoInteracts.CompareTag(PLAYER_TAG))
            {
                inventoryUI.SwitchUI();
                // Trader player = whoInteracts.GetComponent<Trader>();
                // Item itemFromPlayer = player.Inventory.GetItems()[0];
                // if (itemFromPlayer)
                // {
                //     player.Trade(itemFromPlayer, this.trader);
                // }
            }
        }

        private void SellToPlayer(Item item)
        {
            Trader player = GameObject.FindWithTag("Player").GetComponent<Trader>();
            trader.Trade(item, player);
        }
    }
}