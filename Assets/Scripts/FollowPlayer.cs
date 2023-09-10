using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	Transform playerTransform;
	// Start is called before the first frame update
	[SerializeField] float playerpositionx;
	[SerializeField] float playerpositiony;
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
		StartCoroutine("doSm");
		//transform.position = new Vector3(playerTransform.position.x + playerpositionx, playerTransform.position.y + playerpositiony, transform.position.z);
	}

	private IEnumerator doSm()
    {
		Vector3 position = new Vector3(playerTransform.position.x + playerpositionx, playerTransform.position.y + playerpositiony, transform.position.z);
		yield return new WaitForSeconds(0.1f);
		transform.position = position;
    }
}
