using UnityEngine;
using SIM.Core;

namespace SIM.Control
{
    public class ShopkeeperControl : MonoBehaviour, IInteractable
    {
        public void Interact(GameObject whoInteracts, out GameObject interacted)
        {
            interacted = gameObject;
            print(whoInteracts.name + " is trying to interact with me");
        }
    }
}