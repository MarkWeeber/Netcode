using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileFire : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform firePortPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlashGameObject;
    [SerializeField] private Collider2D playerCollider;
    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 30f;
    [SerializeField] private float fireRateInSeconds = 0.75f;
    [SerializeField] private float muzzleFlashDuration = 0.3f;

    private bool firing = false;
    private bool fireCoolDownReady = false;
    private GameObject spawnedProjectile = null;
    private float fireRateTimer;
    private float muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
        inputReader.PrimaryFireEvent.AddListener(HandleFilre);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }
        inputReader.PrimaryFireEvent.RemoveListener(HandleFilre);
    }

    private void Update()
    {
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;
            if (muzzleFlashTimer <= 0f)
            {
                muzzleFlashGameObject.SetActive(false);
            }
        }

        if (!IsOwner) { return; }
        if (!fireCoolDownReady)
        {
            fireRateTimer -= Time.deltaTime;
            if (fireRateTimer < 0)
            {
                fireCoolDownReady = true;
            }
        }
        if (firing && fireCoolDownReady)
        {
            fireRateTimer = fireRateInSeconds;
            fireCoolDownReady = false;
            PrimaryFireServerRpc(firePortPoint.position, firePortPoint.up);
            FireProjectile(firePortPoint.position, firePortPoint.up);
        }
    }

    private void MuzzleFlashShow()
    {

    }

    private void HandleFilre(bool firePressed)
    {
        firing = firePressed;
    }

    private void FireProjectile(Vector3 position, Vector3 direction)
    {
        muzzleFlashGameObject.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;
        spawnedProjectile = Instantiate(clientProjectilePrefab, position, Quaternion.identity);
        spawnedProjectile.transform.up = direction;
        Physics2D.IgnoreCollision(playerCollider, spawnedProjectile.GetComponent<Collider2D>());
        if (spawnedProjectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 position, Vector3 direction)
    {
        spawnedProjectile = Instantiate(serverProjectilePrefab, position, Quaternion.identity);
        spawnedProjectile.transform.up = direction;
        Physics2D.IgnoreCollision(playerCollider, spawnedProjectile.GetComponent<Collider2D>());
        if (spawnedProjectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
        PrimaryFireClientRpc(position, direction);
    }

    [ClientRpc]
    private void PrimaryFireClientRpc(Vector3 position, Vector3 direction)
    {
        if(IsOwner) { return; }
        FireProjectile(position, direction);
    }
}
