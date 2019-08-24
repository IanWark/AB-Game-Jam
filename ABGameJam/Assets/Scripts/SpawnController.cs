using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Can spawn a new thing every newSpawnSize units
    public float spawnDistanceMin = 5;
    public float spawnDistanceMax = 10;
    public float newSpawnSize = 5;
    private float lastXPositionSpawned = 0;

    public DwarfCivilian dwarfCivilian;
    public float dwarfCivilianChance = 1;
    public int dwarfCivilianNum = 2;

    public DwarfMelee dwarfMelee;
    public float dwarfMeleeChance = 1;
    public int dwarfMeleeNum = 1;
    
    public DwarfRanged dwarfRanged;
    public float dwarfRangedChance = 0;
    public int dwarfRangedNum = 0;

    public Building building;
    public float buildingChance = 1;
    public int buildingNum = 1;
    
    // Update is called once per frame
    void Update()
    {
        float cameraX = Globals.mainCamera.transform.position.x;

        if (cameraX >= lastXPositionSpawned + newSpawnSize)
        {
            float newSpawn = Random.Range(0, dwarfCivilianChance + dwarfMeleeChance + buildingChance);

            if (newSpawn <= dwarfCivilianChance)
            {
                Spawn(dwarfCivilian, cameraX, dwarfCivilian.spawnHeight, dwarfCivilianNum);
            } 
            else if (newSpawn <= dwarfCivilianChance + dwarfMeleeChance)
            {
                Spawn(dwarfMelee, cameraX, dwarfMelee.spawnHeight, dwarfMeleeNum);
            }
            else if (newSpawn <= dwarfCivilianChance + dwarfMeleeChance + buildingChance)
            {
                Spawn(building, cameraX, building.spawnHeight, buildingNum);
            }
            
            lastXPositionSpawned = Globals.mainCamera.transform.position.x;
        }        
    }

    void Spawn(Enemy prefab, float spawnX, float spawnY, int numberToSpawn)
    {
        for (int i = 0; i < numberToSpawn; ++i)
        {
            Instantiate(prefab, new Vector2(spawnX + Random.Range(spawnDistanceMin, spawnDistanceMax), spawnY), gameObject.transform.rotation);
        }
    }

}
