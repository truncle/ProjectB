using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveStraight : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 rotation;

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