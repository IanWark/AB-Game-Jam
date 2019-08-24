using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfCivilian : Dwarf
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (detectedPlayer)
            {
                // RUN AWAY!
                rb2d.MovePosition(rb2d.position + new Vector2(moveSpeed * Time.deltaTime, 0));
            } else
            {
                float playerDistance = Globals.player.transform.position.x - transform.position.x;
                if (Mathf.Abs(playerDistance) <= detectionRange)
                {
                    detectedPlayer = true;
                }
            }
        }
    }
}
