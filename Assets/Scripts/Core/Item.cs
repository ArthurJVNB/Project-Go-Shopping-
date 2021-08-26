using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIM.Core
{
    public class Item : MonoBehaviour
    {
        public bool IsStackable { get { return isStackable; } }
        public Sprite UIImage { get { return uiImage; } }

        [SerializeField] bool isStackable;
        [SerializeField] Sprite uiImage;

    }
}
