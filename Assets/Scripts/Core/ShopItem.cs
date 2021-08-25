using UnityEngine;

namespace SIM.Core
{
    public class ShopItem : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            print("You are buying " + name);
        }
    }
}
