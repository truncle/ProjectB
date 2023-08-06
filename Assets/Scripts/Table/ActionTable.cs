using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UI;

public struct ActionData
{
	public int id;
	public string name;
	public ActionType type;
	public int totalFrames;
	public string nextAction;
}

public enum ActionType
{
	State,
	Trigger,
}

static public class ActionTable
{
	static public List<ActionData> testActionTable = new(){
		new(){
			id = 0,
			name = "move",
			type = ActionType.State,
			totalFrames = 0,
		},
		new(){
			id = 1,
			name = "jump",
			type = ActionType.State,
			totalFrames = 0,
		},
	};

	static public Dictionary<string, List<ActionData>> actionTables = new();

	static public ActionData? GetAction(string tableName, string actionName)
	{
		List<ActionData> actionTable = LoadActionTable(tableName);
		var queryActions = actionTable.Where(a => a.name == actionName);
		return queryActions.Any() ? queryActions.First() : null;
	}

	static public ActionData DefaultAction(string tableName)
	{
		List<ActionData> actionTable = LoadActionTable(tableName);
		return actionTable.First();
	}

	static private List<ActionData> LoadActionTable(string tableName)
	{
		if (!actionTables.ContainsKey(tableName))
		{
			//todo 加载actionTable
			actionTables[tableName] = testActionTable;
		}
		return actionTables[tableName];
	}
}

