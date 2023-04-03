using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] public Camera mainCamera;

	[SerializeField] Transform followObject;

	// Start is called before the first frame update
	void Start()
	{
		GameObject player = GameObject.Find("Player");
		if (player != null)
		{
			followObject = player.transform;
		}

	}

	// Update is called once per frame
	void Update()
	{
		if (followObject != null)
		{
			transform.position = new Vector3(followObject.position.x, followObject.position.y, transform.position.z);
		}
	}

	public void Follow(string name)
	{
		followObject = GameObject.Find(name).transform;
	}
}
