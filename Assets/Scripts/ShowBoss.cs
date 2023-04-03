using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBoss : MonoBehaviour
{
    [SerializeField] Animator bossAnimator;

    [SerializeField] CameraController cameraController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        cameraController.Follow("Boss");
        bossAnimator.CrossFade("AttackShow", 0);
        Invoke("EndShow", 1f);
    }

    public void EndShow()
    {
        bossAnimator.CrossFade("Idle", 0);
        cameraController.Follow("Player");
    }

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player") {
            Show();
        }
	}
}
