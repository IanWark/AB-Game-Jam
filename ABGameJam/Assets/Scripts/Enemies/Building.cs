using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Enemy
{
    public float spawnHeight = -0.65f;

    public float attackSpeed = 3;
    private float attackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        


    }

    void FixedUpdate()
    {
        if (GetDetectedPlayer())
        {
            attackTimer += Time.fixedDeltaTime;
            
        }
    }

    public override void OnHit(int damage)
    {
        // TODO 
        Destroy(gameObject);
    }
}
