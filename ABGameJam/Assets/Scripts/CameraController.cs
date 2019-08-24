using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5;

    public bool controlEnabled = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            bool right = Input.GetKey(KeyCode.L);
            bool left = Input.GetKey(KeyCode.J);

            int direction = (right ? 1 : 0) - (left ? 1 : 0);

            // Simple x-axis only movement
            transform.Translate(new Vector3(direction * speed * Time.deltaTime, 0, 0));
        }
    }
}
