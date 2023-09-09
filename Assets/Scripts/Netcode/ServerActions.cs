using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerActions : MonoBehaviour
{
    public void JoinHost()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void CreateHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}
