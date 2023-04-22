using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public struct ScriptData
{
	public int id;
	public string text;
	public ScriptType type;
	public List<ScriptDialog> dialog;

	public string PrintScript()
	{
		Debug.Log(text);
		if (dialog != null){
			StringBuilder sb = new();
			foreach (ScriptDialog statement in dialog)
			{
				sb.AppendLine(string.Format("character{0}: {1}", statement.characterId, statement.text));
			}
			Debug.Log(sb.ToString());
			return sb.ToString();
		}
		return null;
	}
}

public struct ScriptDialog
{
	public string text;
	public int characterId; //人物
	public int expressionId; //表情
}

public enum ScriptType
{
	Talk, Statement
}

public static class ScriptTable
{
	static public List<ScriptData> scripts = new(){
		new(){
			id = 0,
			text = "空脚本"
		},
		new(){
			id = 1,
			text = "这是一段陈述"
		},
		new(){
			id = 2,
			text = "这是一段对话",
			dialog = new List<ScriptDialog>()
			{
				new(){ text = "我要快些回家", characterId = 1 , expressionId = 0},
			}
		},
	};

	static public ScriptData GetScript(int scriptId)
	{
		return scripts[scriptId];
	}
}
