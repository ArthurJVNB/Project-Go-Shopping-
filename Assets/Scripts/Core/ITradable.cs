namespace SIM.Core
{
    internal interface ITradable
    {
        float GetPrice();
        bool TryToTrade(Trader buyer);
    }
}