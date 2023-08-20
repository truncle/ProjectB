using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public struct ScriptData<T>
{
	public int id;
	public List<T> data;

	public string PrintScript()
	{
		if (data != null)
		{
			StringBuilder sb = new();
			foreach (T dataLine in data)
			{
				sb.AppendLine(dataLine.ToString());
			}
			Debug.Log(sb.ToString());
			return sb.ToString();
		}
		return null;
	}
}

public struct ScriptDialog
{
	public int id;
	public string text;
	public int characterId; //人物
	public int expressionId; //表情
	public bool finishSign;

	public override string ToString()
	{
		return string.Format("character{0}: {1}", characterId, text);
	}
}

public static class ScriptTable
{
	static public List<ScriptData<ScriptDialog>> dialogTable = new()
	{
		new()
		{
			id = 0,
			data = new()
			{
				new() { id = 0, text = "测试1", characterId = 1, expressionId = 0 },
			}
		},
		new()
		{
			id = 1,
			data = new()
			{
				new() { id = 0, text = "测试2", characterId = 1, expressionId = 0 },
			}
		},
		new()
		{
			id = 2,
			data = new()
			{
				new() { id = 0, text = "我要快些回家", characterId = 1, expressionId = 0, finishSign = false },
				new() { id = 1, text = "似乎有什么事情发生了", characterId = 1, expressionId = 0, finishSign = true },
			}
		},
		new()
		{
			id = 3,
			data = new()
			{
				new() { id = 0, text = "看上去形势不妙", characterId = 1, expressionId = 0, finishSign = false },
				new() { id = 1, text = "希望我能赶得上", characterId = 1, expressionId = 0, finishSign = true },
			}
		},
	};

	static public ScriptData<T> GetScript<T>(int scriptId, ScriptType type)
	{
		object data = null;
		switch (type)
		{
			case ScriptType.Dialog: data = dialogTable[scriptId]; break;
		}
		return (ScriptData<T>)Convert.ChangeType(data, typeof(ScriptData<T>));
	}

	static public void LoadDialog()
	{
		//todo 先写在这里, 之后挪到GameConfig里, 可能有多张表需要同时读取
		//todo 还要再详细理一下, 有不够明确的的地方
		var path = "dialog.csv";
		var rawData = Util.ReadCsv(path);
		Dictionary<int, ScriptData<ScriptDialog>> resultTmp = new();
		foreach (var pair in rawData)
		{
			var dialogId = pair.Key;
			var lineRawData = pair.Value;
			int scriptId = Convert.ToInt32(lineRawData["groupId"]);
			var scriptData = resultTmp.ContainsKey(scriptId) ? resultTmp[scriptId] : new ScriptData<ScriptDialog>()
			{
				id = scriptId,
				data = new(),
			};
			ScriptDialog dialogData = new()
			{
				id = dialogId,
				text = lineRawData["text"], //todo 这里需要读翻译表
				characterId = Convert.ToInt32(lineRawData["characterId"]),
				expressionId = Convert.ToInt32(lineRawData["expressionId"]),
				finishSign = Convert.ToBoolean(int.Parse(lineRawData["isFinish"])),
			};
			scriptData.data.Add(dialogData);
			resultTmp[scriptId] = scriptData;
		}
		foreach(var pair in resultTmp)
		{
			Debug.Log(pair.Value.PrintScript());
		}
	}
}
