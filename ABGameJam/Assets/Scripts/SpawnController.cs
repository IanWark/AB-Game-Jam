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

    public Building building1_1;
    public float building1_1Chance = 1;
    public int building1_1Num = 1;

    public Building building1_2;
    public float building1_2Chance = 1;
    public int building1_2Num = 1;

    public Building building1_3;
    public float building1_3Chance = 1;
    public int building1_3Num = 1;

    public Building building2_1;
    public float building2_1Chance = 0;
    public int building2_1Num = 1;

    public Building building2_2;
    public float building2_2Chance = 0;
    public int building2_2Num = 1;

    // Update is called once per frame
    void Update()
    {
        float cameraX = Globals.mainCamera.transform.position.x;

        if (cameraX >= lastXPositionSpawned + newSpawnSize)
        {
            float newSpawn = Random.Range(0, dwarfCivilianChance + dwarfMeleeChance + building1_1Chance + building1_2Chance + building1_3Chance + building2_1Chance + building2_2Chance);

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
