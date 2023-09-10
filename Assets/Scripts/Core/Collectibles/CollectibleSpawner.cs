using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CollectibleSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private int maxCollectibles = 10;
    [SerializeField] private int coinsValue = 3;
    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private LayerMask spawnCheckLayer;

    private GameObject spawnedPrefab;
    private Collider2D[] collisions = new Collider2D[1];
    private float coinRadius;
    private int collisionsOnSpawnPoint;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        coinRadius = collectiblePrefab.GetComponent<CircleCollider2D>().radius;
        for (int i = 0; i < maxCollectibles; i++)
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        spawnedPrefab = Instantiate(collectiblePrefab, GetRandomSpawnPoint(), Quaternion.identity);
        if (spawnedPrefab.TryGetComponent<RespawningCoin>(out RespawningCoin respawningCoin))
        {
            respawningCoin.SetValue(coinsValue);
            respawningCoin.OnCollected.AddListener(HandleCollected);
        }
        if (spawnedPrefab.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
        {
            networkObject.Spawn();
        }
    }

    private void HandleCollected(RespawningCoin respawningCoin)
    {
        respawningCoin.transform.position = GetRandomSpawnPoint();
        respawningCoin.ResetCoin();
    }

    private Vector2 GetRandomSpawnPoint()
    {
        float x = 0f;
        float y = 0f;
        Vector2 spawnPoint = Vector2.zero;
        while (true)
        {
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            spawnPoint = new Vector2(x, y);
            collisionsOnSpawnPoint = Physics2D.OverlapCircleNonAlloc(spawnPoint, coinRadius, collisions, spawnCheckLayer);
            if (collisionsOnSpawnPoint == 0)
            {
                return spawnPoint;
            }
        }
    }
}

