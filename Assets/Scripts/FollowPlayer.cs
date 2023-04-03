using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	Transform playerTransform;
	// Start is called before the first frame update
	void Start()
	{
		playerTransform = GameObject.Find("Player").GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
	}

	private void LateUpdate()
	{
		transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
	}
}
