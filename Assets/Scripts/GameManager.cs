using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;

	public static GameManager Instance
	{
		get { return instance; }
	}

	private void Awake()
	{
		LoadTables();
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		InputManager.Update();
	}

	//游戏启动时读取的表
	private void LoadTables()
    {
		ScriptTable.LoadTable();
    }
}
