namespace SIM.Core
{
    internal interface ITradable
    {
        Trader GetOwner();
        void SetOwner(Trader newOwner);
        float GetPrice();
        bool TryToTrade(Trader buyer, out Item boughtItem);
    }
}