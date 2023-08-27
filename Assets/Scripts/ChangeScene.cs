using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	[SerializeField] private string sceneName;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			SceneManager.LoadSceneAsync(sceneName);
		}
		collision.transform.position = Vector3.zero;
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
	}
}
