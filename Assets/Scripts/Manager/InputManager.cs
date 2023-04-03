﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InputType
{
	//Move
	Left, Right, Up, Down,
	//Action
	Attack, Jump, Dash, Skill,
	Interact, UseItem,
	//UI
	OK, Cancel
}

public static class InputManager
{
	//添加更多键位映射
	private static Dictionary<KeyCode, InputType> keyMap = new()
	{
		{KeyCode.A, InputType.Left },
		{KeyCode.D, InputType.Right },
		{KeyCode.W, InputType.Up },
		{KeyCode.S, InputType.Down },

		{KeyCode.Space, InputType.Jump },
		{KeyCode.LeftShift, InputType.Dash },
		{KeyCode.F, InputType.Skill },
		{KeyCode.E, InputType.Interact },
		{KeyCode.R, InputType.UseItem },

		{KeyCode.KeypadEnter, InputType.OK },
		{KeyCode.Escape, InputType.Cancel },
	};

	private static List<InputType> InputList = new();
	private static List<InputType> KeyDownList = new();
	private static List<InputType> KeyUpList = new();

	//每次Update的时候更新一下输入列表
	public static void Update()
	{
		InputList.Clear();
		KeyDownList.Clear();
		KeyUpList.Clear();
		//keyboard input
		foreach (var keyPair in keyMap)
		{
			if (Input.GetKey(keyPair.Key))
			{
				InputList.Add(keyPair.Value);
			}
			if (Input.GetKeyDown(keyPair.Key))
			{
				KeyDownList.Add(keyPair.Value);
			}
			if (Input.GetKeyUp(keyPair.Key))
			{
				KeyUpList.Add(keyPair.Value);
			}
		}

		//mouse input
		if (Input.GetMouseButton(0))
		{
			InputList.Add(InputType.Attack);
		}
		if (Input.GetMouseButton(1))
		{
			InputList.Add(InputType.Dash);
		}
	}

	//获取输入
	public static bool GetInput(InputType input)
	{
		return InputList.Contains(input);
	}
	public static bool GetKeyDown(InputType input)
	{
		return KeyDownList.Contains(input);
	}
	public static bool GetKeyUp(InputType input)
	{
		return KeyUpList.Contains(input);
	}

	//额外添加输入
	public static void AddInput(InputType input)
	{
		InputList.Add(input);
	}
	public static void AddKeyDown(InputType input)
	{
		KeyDownList.Add(input);
	}
	public static void AddKeyUp(InputType input)
	{
		KeyUpList.Add(input);
	}
}
