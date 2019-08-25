using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Can spawn a new thing every newSpawnSize units
    private float spawnDistanceMin = 3;
    private float spawnDistanceMax = 8;
    private float newSpawnSize = 5;
    private float lastXPositionSpawned = 0;

    public DwarfCivilian dwarfCivilian;
    private float dwarfCivilianChance = 1;
    private int dwarfCivilianNum = 2;

    public DwarfMelee dwarfMelee;
    private float dwarfMeleeChance = 0;
    private int dwarfMeleeNum = 1;

    public Building building1_1;
    private float building1_1Chance = 0;
    private int building1_1Num = 1;

    public Building building1_2;
    private float building1_2Chance = 0;
    private int building1_2Num = 1;

    public Building building1_3;
    private float building1_3Chance = 0;
    private int building1_3Num = 1;

    public Building building2_1;
    private float building2_1Chance = 0;
    private int building2_1Num = 1;

    public Building building2_2;
    private float building2_2Chance = 0;
    private int building2_2Num = 1;

    public Helicopter helicopter;
    private float helicopterChance = 0;
    private int helicopterNum = 1;

    // Update is called once per frame
    void Update()
    {
        float cameraX = Globals.mainCamera.transform.position.x;

        IncreaseSpawnRates(cameraX);

        if (cameraX >= lastXPositionSpawned + newSpawnSize)
        {
            float newSpawn = Random.Range(0, dwarfCivilianChance + dwarfMeleeChance + building1_1Chance + building1_2Chance + building1_3Chance + building2_1Chance + building2_2Chance + helicopterChance);

            if (newSpawn <= dwarfCivilianChance)
            {
                Spawn(dwarfCivilian, cameraX, dwarfCivilian.spawnHeight, dwarfCivilianNum);
            } 
            else if (newSpawn <= dwarfCivilianChance + dwarfMeleeChance)
            {
                Spawn(dwarfMelee, cameraX, dwarfMelee.spawnHeight, dwarfMeleeNum);
            }
            else if (newSpawn <= dwarfCivilianChance + dwarfMeleeChance + building1_1Chance)
            {
                Spawn(building1_1, cameraX, building1_1.spawnHeight, building1_1Num);
            }
            else if (newSpawn <= dwarfCivilianChance + dwarfMeleeChance + building1_1Chance + building1_2Chance)
            {
                Spawn(building1_2, cameraX, building1_2.spawnHeight, building1_2Num);
            }
            else if (newSpawn <= dwarfCivilianChance + dwarfMeleeChance + building1_1Chance + building1_2Chance + building1_3Chance)
            {
                Spawn(building1_3, cameraX, building1_3.spawnHeight, building1_3Num);
            }
            else if (newSpawn <= dwarfCivilianChance + dwarfMeleeChance + building1_1Chance + building1_2Chance + building1_3Chance + building2_1Chance)
            {
                Spawn(building2_1, cameraX, building2_1.spawnHeight, building2_1Num);
            }
            else if (newSpawn <= dwarfCivilianChance + dwarfMeleeChance + building1_1Chance + building1_2Chance + building1_3Chance + building2_1Chance + building2_2Chance)
            {
                Spawn(building2_2, cameraX, building2_2.spawnHeight, building2_2Num);
            }
            else if (newSpawn <= dwarfCivilianChance + dwarfMeleeChance + building1_1Chance + building1_2Chance + building1_3Chance + building2_1Chance + building2_2Chance + helicopterChance)
            {
                Spawn(helicopter, cameraX, helicopter.spawnHeight, helicopterNum);
            }

            lastXPositionSpawned = Globals.mainCamera.transform.position.x;
        }        
    }

    void IncreaseSpawnRates(float cameraX)
    {
        // Spawn melee
        if (cameraX > 5)
        {
            dwarfMeleeChance = 1;
        }
        // Spawn buildings
        if (cameraX > 15) {
            newSpawnSize = 4;

            building1_1Chance = 0.1f;
            building1_2Chance = 0.1f;
            building1_3Chance = 0.1f;
        }
        // Spawn more melee continuously
        if (cameraX > 50)
        {
            dwarfMeleeNum = Mathf.FloorToInt(cameraX / 60);
            dwarfCivilianNum = Mathf.FloorToInt(cameraX / 60);
        }
        // Spawn helicopters
        if (cameraX > 75)
        {
            helicopterChance = 0.01f;
        }
        // Spawn double buildings
        if (cameraX > 100)
        {
            newSpawnSize = 3;
            building2_1Chance = 0.3f;
            building2_2Chance = 0.3f;

            building1_1Chance = 0.1f;
            building1_2Chance = 0.1f;
            building1_3Chance = 0.1f;
        }
        // Reduce civilian spawn chance
        if (cameraX > 130)
        {
            dwarfCivilianChance = 60f/cameraX;

            helicopterChance = 1;
            dwarfMeleeChance = 1;
            building2_1Chance = 0.5f;
            building2_2Chance = 0.5f;
        }
        // even smaller spawn size, may cause chaos
        // Spawn only double buildings
        if (cameraX > 200)
        {
            newSpawnSize = 2;

            building1_1Chance = 0;
            building1_2Chance = 0;
            building1_3Chance = 0;
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
