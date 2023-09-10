using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawningCoin : Coin
{
    public UnityEvent<RespawningCoin> OnCollected;
    private Vector3 previousPosition;

    private void Update()
    {
        UpdateCoinPosition();
    }

    private void UpdateCoinPosition()
    {
        if (previousPosition != transform.position)
        {
            Show(true);
        }
        previousPosition = transform.position;
    }

    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }
        if (alreadyCollected)
        {
            return 0;
        }
        alreadyCollected = true;
        OnCollected?.Invoke(this);
        return coinValue;
    }

    public void ResetCoin()
    {
        alreadyCollected = false;
    }
}
