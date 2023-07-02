using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEventObject))]
public class GameEventEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GameEventObject gameEventObject = (GameEventObject)target; // 获取当前编辑的组件

		if (!gameEventObject.GetComponent<Collider2D>())
		{
			BoxCollider2D collider = gameEventObject.gameObject.AddComponent<BoxCollider2D>();
			collider.isTrigger = true;
		}
	}
}
