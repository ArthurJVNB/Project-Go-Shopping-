using UnityEngine;
using SIM.Core;

namespace SIM.Control
{
    [RequireComponent(typeof(Trader))]
    public class ShopkeeperControl : MonoBehaviour, IInteractable
    {
        Trader trader;

        private void Awake()
        {
            trader = GetComponent<Trader>();
        }

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
    }
}