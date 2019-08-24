using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Can spawn a new thing every newSpawnSize units
    public float spawnDistanceMin = 5;
    public float spawnDistanceMax = 8;
    public float newSpawnSize = 5;
    private float lastXPositionSpawned = 0;

    public DwarfMelee dwarfMelee;

    // Update is called once per frame
    void Update()
    {
        float cameraX = Globals.mainCamera.transform.position.x;

        if (cameraX >= lastXPositionSpawned + newSpawnSize)
        {
            Instantiate(dwarfMelee, new Vector2(cameraX + Random.Range(spawnDistanceMin, spawnDistanceMax), dwarfMelee.spawnHeight), gameObject.transform.rotation);



            lastXPositionSpawned = Globals.mainCamera.transform.position.x;
        }        
    }
}
