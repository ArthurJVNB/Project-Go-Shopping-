using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIM.Core
{
    public class RangeDetector : MonoBehaviour
    {
        public Action<Collider2D, Collider2D> onTriggerEnter2D;
        public Action<Collider2D, Collider2D> onTriggerExit2D;

        Collider2D myCollider2D;

        private void Awake()
        {
            myCollider2D = GetComponent<Collider2D>();

            if (!myCollider2D)
            {
#if UNITY_EDITOR
                Debug.LogWarning(this.GetType() + " doesn't have a trigger2D attached to it. It won't work without it!");
#endif
                return;
            }

            myCollider2D.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            onTriggerEnter2D?.Invoke(myCollider2D, other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            onTriggerExit2D?.Invoke(myCollider2D, other);
        }
    }
}
