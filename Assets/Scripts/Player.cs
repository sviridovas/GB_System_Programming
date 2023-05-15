using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject playerCharacter;
    private void Start()
    {
        SpawnCharacter();
    }

    public void SpawnCharacter()
    {
        // if (!isServer)
        // {
        //     return;
        // }
        playerCharacter = Instantiate(playerPrefab);
        // NetworkServer.SpawnWithClientAuthority(playerCharacter, connectionToClient);
    }
}
