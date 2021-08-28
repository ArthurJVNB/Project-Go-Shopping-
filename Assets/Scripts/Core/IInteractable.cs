using UnityEngine;

namespace SIM.Core
{
    public interface IInteractable
    {
        void Interact(GameObject whoInteracts, out GameObject interacted);
        void ShowHint();
    }
}
