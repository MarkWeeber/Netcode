using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [SerializeField] private LayerMask targetMask;
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();
    private Coin triggeredCoin;
    private int triggeredCoinValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
        {
            if (collision.TryGetComponent<Coin>(out triggeredCoin))
            {
                triggeredCoinValue = triggeredCoin.Collect();
                if (IsServer)
                {
                    TotalCoins.Value += triggeredCoinValue;
                }
            }
        }
    }
}
