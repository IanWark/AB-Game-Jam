using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Enemy
{
    public float spawnHeight = -0.65f;
    public float arrowSpawnExtraY = 0.4f;
    public float arrowSpawnExtraX = -0.1f;

    public Arrow arrow;
    public float attackSpeed = 3;
    private float attackTimer = 0;
    
    private int scoreValue = 1500;

    // Start is called before the first frame update
    void Start()
    {
        detectionRange = 10;
    }

    void FixedUpdate()
    {
        if (GetDetectedPlayer())
        {
            attackTimer += Time.fixedDeltaTime;
            
            if (attackTimer > attackSpeed)
            {
                Vector2 ourPos = new Vector2(transform.position.x + arrowSpawnExtraX, transform.position.y + arrowSpawnExtraY);
                Vector2 theirPos = new Vector2(Globals.player.transform.position.x, Globals.player.transform.position.y);

                Vector2 arrowDirection = theirPos - ourPos;
                arrowDirection.Normalize();

                Arrow newArrow = Instantiate(arrow, ourPos, Quaternion.identity);
                newArrow.Direction = arrowDirection;

                attackTimer = 0;
            } 
        }
    }

    public override void OnHit(int damage)
    {
        // TODO 
        
        // Increase score (TODO: when dead or per hit?)
        GameObject Player = GameObject.Find("Player");
        PlayerController playerController = Player.GetComponent<PlayerController>();
        playerController.score += scoreValue;
        
        Destroy(gameObject);
    }
}
