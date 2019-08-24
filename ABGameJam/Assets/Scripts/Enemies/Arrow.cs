using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifetime = 10;
    static public float damage = 10;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }


}
