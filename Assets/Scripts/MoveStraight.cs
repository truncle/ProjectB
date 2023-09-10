using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveStraight : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 rotation;
    public float LifeTime = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }

}
