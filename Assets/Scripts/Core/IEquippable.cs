namespace SIM.Core
{
    public interface IEquippable<T>
    {
        void Equip();
        void Unequip();
    }
}