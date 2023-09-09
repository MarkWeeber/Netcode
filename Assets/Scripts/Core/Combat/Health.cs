using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int maxHealth { get; private set; } = 100;

    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    private bool isDead = false;

    public UnityEvent<Health> OnDie;

    public UnityEvent<float, float> OnHealthChanged;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        CurrentHealth.Value = maxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
    }

    private void ModifyHealth(int value)
    {
        if (isDead)
        {
            return;
        }
        CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value + value, 0, maxHealth);
        if (CurrentHealth.Value == 0)
        {
            isDead = true;
            OnDie?.Invoke(this);
        }
    }
}
