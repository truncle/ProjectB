using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		InputManager.Update();
	}
}
