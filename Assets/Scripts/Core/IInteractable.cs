using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIM.Core
{
    public interface IInteractable
    {
        void Interact(out GameObject interactedGameObject);
    }
}
