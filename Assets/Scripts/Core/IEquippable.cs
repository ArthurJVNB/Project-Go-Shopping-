namespace SIM.Core
{
    public interface IEquippable<T>
    {
        bool Equip(T whoIsTryingToEquip, out EquipmentSlot slotToPut);
        void Unequip();
    }
}