using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct ActionData
{
	public ActionId id;
	public ActionType type;
	public int totalFrames;
	public ActionId? nextId;
	public List<CancelInfo> cancelInfos;
	public List<InputType> inputList; //输入方式
}

public struct CancelInfo
{
	public int startFrame;
	public int endFrame;

	public ActionId actionId;
	public int toFrame;
}

public enum ActionType
{
	State,
	Trigger,
}

public enum ActionId
{
	Move, Jump, Dash, ComboAttack01, ComboAttack02, ComboAttack03,
}

static public class ActionTable
{
	static public List<ActionData> actions = new(){
		new(){
			id = ActionId.Move,
			type = ActionType.State,
			totalFrames = 0,
			cancelInfos = new List<CancelInfo>(){},
		},
		new(){
			id = ActionId.Jump,
			type = ActionType.State,
			totalFrames = 0,
			cancelInfos = new List<CancelInfo>(){}
		},
		new(){
			id = ActionId.ComboAttack01,
			type = ActionType.Trigger,
			totalFrames = 25,
			cancelInfos = new List<CancelInfo>(){},
			inputList = new(){InputType.Attack}
		},
		new(){
			id = ActionId.ComboAttack02,
			type = ActionType.Trigger,
			totalFrames = 21,
			cancelInfos = new List<CancelInfo>(){},
			inputList = new(){InputType.Attack}
		},
		new(){
			id = ActionId.ComboAttack03,
			type = ActionType.Trigger,
			totalFrames = 17,
			cancelInfos = new List<CancelInfo>(){},
			inputList = new(){InputType.Attack}
		},
	};

	static public ActionData GetAction(ActionId actionId)
	{
		var queryActions = actions.Where(a => a.id == actionId);
		return queryActions.First();
	}

	static public ActionData GetStateAction()
	{
		//获取当前的状态动作
		return GetAction(ActionId.Move);
	}
}

