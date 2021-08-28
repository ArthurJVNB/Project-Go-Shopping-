using UnityEngine;

namespace SIM.Core
{
    public class Equipment : MonoBehaviour
    {
        [SerializeField] SpriteRenderer torsoSlot;
        [SerializeField] SpriteRenderer legsSlot;

        public void Equip(Item item)
        {
            EquipmentSlot slot = item.EquipmentSlot;
            Sprite spriteToUse = item.EquipedImage;

            switch (slot)
            {
                case EquipmentSlot.Torso:
                    EquipTorso(spriteToUse);
                    break;
                case EquipmentSlot.Legs:
                    EquipLegs(spriteToUse);
                    break;
                default:
                    break;
            }
        }

        public void Unequip(Item item)
        {
            item.Unequip();

            switch (item.EquipmentSlot)
            {
                case EquipmentSlot.Torso:
                    UnequipTorso();
                    break;
                case EquipmentSlot.Legs:
                    UnequipLegs();
                    break;
                default:
                    break;
            }
        }

        private void EquipTorso(Sprite sprite)
        {
            torsoSlot.sprite = sprite;
        }

        private void UnequipTorso()
        {
            torsoSlot.sprite = null;
        }

        private void EquipLegs(Sprite sprite)
        {
            legsSlot.sprite = sprite;
        }

        private void UnequipLegs()
        {
            legsSlot.sprite = null;
        }
    }
}