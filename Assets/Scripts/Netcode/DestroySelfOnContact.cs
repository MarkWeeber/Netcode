using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DestroySelfOnContact : MonoBehaviour
{
    [SerializeField] private LayerMask targetMask;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Utils.CheckLayer(targetMask, other.gameObject.layer))
        {
            Destroy(gameObject);
        }
    }
}
