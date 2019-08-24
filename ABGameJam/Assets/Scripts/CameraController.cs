using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool controlEnabled = false;
    public float controlSpeed = 5;
    public FreeParallax parallax;

    // Start is called before the first frame update
    void Start()
    {
        Globals.mainCamera = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (controlEnabled)
        {
            // Independant camera movement (for testing only)
            bool right = Input.GetKey(KeyCode.P);
            bool left = Input.GetKey(KeyCode.I);

            int direction = (right ? 1 : 0) - (left ? 1 : 0);

            // Simple x-axis only movement
            transform.Translate(new Vector3(direction * controlSpeed * Time.deltaTime, 0, 0));
        } else
        {
            // If player is infront of camera, follow
            // Otherwise, don't move
            float playerX = Globals.player.transform.position.x + 1; // Add in more to give extra space to the right
            if (playerX - transform.position.x > 0)
            {
                transform.position = new Vector3(playerX, transform.position.y, transform.position.z);

                parallax.Speed = -Globals.player.getCurrentSpeed();
            }
            else
            {
                parallax.Speed = 0;
            }
        }
    }
}
