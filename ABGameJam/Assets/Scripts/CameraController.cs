using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool controlEnabled = false;
    public float controlSpeed = 5;
    public FreeParallax parallax;

    // How long the object should shake for.
    public float shakeDuration = .15f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;
    public bool shake;

    Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        Globals.mainCamera = this;
        shake = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (controlEnabled)
        {
            // Independant camera movement (for testing only)
            bool up = Input.GetKey(KeyCode.P);
            bool down = Input.GetKey(KeyCode.Colon);
            bool right = Input.GetKey(KeyCode.Quote);
            bool left = Input.GetKey(KeyCode.L);

            int directionX = (right ? 1 : 0) - (left ? 1 : 0);
            int directionY = (up ? 1 : 0) - (down ? 1 : 0);

            // Simple x-axis only movement
            transform.Translate(new Vector3(directionX * controlSpeed * Time.deltaTime, directionY * controlSpeed * Time.deltaTime, 0));
        }
        else
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

            // Camera shake
            if (shake == true)
            {
                if (shakeDuration > 0)
                {
                    parallax.transform.position = originalPos + Random.insideUnitSphere * shakeAmount;

                    shakeDuration -= Time.deltaTime * decreaseFactor;
                }
                else
                {
                    Shake(false);
                }
            }
            else
            {
                parallax.transform.position = new Vector3(parallax.transform.position.x, 0, parallax.transform.position.z);
            }

        }
    }

    //d is dash or not dash
    public void Shake(bool val)
    {
        if (val)
        {
            shake = val;
            originalPos = parallax.transform.position;
        }
        else
        {
            shake = false;
            shakeDuration = .15f;
            parallax.transform.position = originalPos;
        }

    }
}
