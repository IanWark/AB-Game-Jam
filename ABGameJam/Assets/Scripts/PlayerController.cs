using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;

    public bool controlEnabled = true;

    void Awake()
    {
        Globals.player = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            // This may be useful if we want to make the movement better: https://roystan.net/articles/character-controller-2d.html

            // Simple x-axis only movement
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0));
        }
    }
}
