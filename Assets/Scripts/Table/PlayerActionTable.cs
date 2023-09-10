using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UI;

public struct PlayerActionData
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

	public bool CanCancel(ActionId actionId, int currFrame)
	{
		return this.actionId == actionId && startFrame <= currFrame && endFrame >= currFrame;
	}
}

public enum ActionId
{
	Move, Jump, Dash, ComboAttack01, ComboAttack02, ComboAttack03, Charge1, Charge2, ChargeAttack1, ChargeAttack2,
}

static public class PlayerActionTable
{
	static public List<PlayerActionData> actions = new(){
		new(){
			id = ActionId.Move,
			type = ActionType.State,
			totalFrames = 0,
		},
		new(){
			id = ActionId.Jump,
			type = ActionType.State,
			totalFrames = 0,
			inputList = new(){InputType.Jump},
		},
		new()
		{
			id = ActionId.Dash,
			type = ActionType.Trigger,
			totalFrames = 10,
			inputList = new(){InputType.Dash},
		},
        new()
        {
            id = ActionId.ComboAttack01,
            type = ActionType.Trigger,
            totalFrames = 25,
            cancelInfos = new List<CancelInfo>() { },
            inputList = new() { InputType.Attack }
        },
        //new(){
        //	id = ActionId.ComboAttack02,
        //	type = ActionType.Trigger,
        //	totalFrames = 21,
        //	cancelInfos = new List<CancelInfo>(){},
        //	inputList = new(){InputType.Attack}
        //},
        //new(){
        //	id = ActionId.ComboAttack03,
        //	type = ActionType.Trigger,
        //	totalFrames = 17,
        //	cancelInfos = new List<CancelInfo>(){},
        //	inputList = new(){InputType.Attack}
        //},
    };

	static public PlayerActionData? GetAction(ActionId actionId)
	{
		var queryActions = actions.Where(a => a.id == actionId);
		return queryActions.Any() ? queryActions.First() : null;
	}
}

