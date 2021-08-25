using UnityEngine;

public interface IPurchasable
{
    float GetPrice();
    bool TryToBuy(float buyersMoney, out GameObject boughtObject);
}