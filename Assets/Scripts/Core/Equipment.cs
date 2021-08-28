using UnityEngine;

namespace SIM.Core
{
    public class Equipment : MonoBehaviour
    {
        [SerializeField] SpriteRenderer torsoSlot;
        [SerializeField] SpriteRenderer legsSlot;

        Item torsoItem;
        Item legsItem;

        public void Equip(Item item)
        {
            item.Equip();

            EquipmentSlot slot = item.EquipmentSlot;
            Sprite spriteToUse = item.EquipedImage;

            switch (slot)
            {
                case EquipmentSlot.Torso:
                    EquipTorso(item, spriteToUse);
                    break;
                case EquipmentSlot.Legs:
                    EquipLegs(item, spriteToUse);
                    break;
                default:
                    break;
            }
        }

        public void Unequip(Item item)
        {
            if (item == null) return;

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

        private void EquipTorso(Item item, Sprite sprite)
        {
            if (torsoItem) Unequip(torsoItem);
            torsoItem = item;
            torsoSlot.sprite = sprite;
        }

        private void UnequipTorso()
        {
            torsoItem = null;
            torsoSlot.sprite = null;
        }

        private void EquipLegs(Item item, Sprite sprite)
        {
            if (legsItem) Unequip(legsItem);
            legsItem = item;
            legsSlot.sprite = sprite;
        }

        private void UnequipLegs()
        {
            legsItem = null;
            legsSlot.sprite = null;
        }

    }
}