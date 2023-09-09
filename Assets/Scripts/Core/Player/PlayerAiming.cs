using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] private Transform turretTransform;
    [SerializeField] private InputReader inputReader;

    private void LateUpdate()
    {
        if(!IsOwner) { return; }
        Vector2 aimOnScreenPosition = inputReader.AimPosition;
        Vector2 aimWorldPositon = Camera.main.ScreenToWorldPoint(aimOnScreenPosition);
        turretTransform.up = new Vector2(
            aimWorldPositon.x - turretTransform.position.x,
            aimWorldPositon.y - turretTransform.position.y);
    }
}
