using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Can spawn a new thing every newSpawnSize units
    public float newSpawnSize = 5;
    private float lastXPositionSpawned = 0;

    // Update is called once per frame
    void Update()
    {
        if (Globals.mainCamera.transform.position.x >= lastXPositionSpawned + newSpawnSize)
        {

        }        
    }
}
