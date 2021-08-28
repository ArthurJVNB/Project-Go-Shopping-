namespace SIM.Core
{
    public interface IEquippable<T>
    {
        void Equip();
        // void Equip(out EquipmentSlot equipmentSlot);
        // bool Equip(T whoIsTryingToEquip, out EquipmentSlot slotToPut);
        void Unequip();
    }
}