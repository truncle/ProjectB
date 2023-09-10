using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private ActionController actionController;

    // Start is called before the first frame update
    void Start()
    {
        actionController = GetComponent<ActionController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            actionController.DoAction("attacked");
        }
    }
}
