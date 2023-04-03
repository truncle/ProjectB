using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
	private Animator animator;

	public int CurrentFrame { get; set; }

	public ActionData CurrentAction { get; set; }

	public float speedX = 0;
	public float speedY = 0;

	private List<ActionId> inputActions = new();

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
	}

	void FixedUpdate()
	{
		CurrentFrame++;
		//自然切换Action
		if (CurrentAction.type != ActionType.State && CurrentFrame > CurrentAction.totalFrames)
		{
			ActionData nextAction;
			if (CurrentAction.nextId.HasValue)
				nextAction = ActionTable.GetAction(CurrentAction.nextId.Value);
			else
				nextAction = ActionTable.GetStateAction();
			DoAction(nextAction);
		}

		//输入切换Action, 需要检查是否可以Cancel
		if (inputActions.Count > 0)
			Debug.Log(inputActions.First());
		CheckCancel(inputActions);
		inputActions.Clear();

		//事件切换Action, 如受击, 死亡。 强制切换
		if (true) //EventExist
		{
			//getEventAction()
		}
	}

	// todo 实现Cancel功能
	void CheckCancel(List<ActionId> actionIds)
	{
		if (actionIds.Count > 0)
		{
			DoAction(actionIds.First());
		}
	}

	public void AddInputAction(List<ActionId> actionIds)
	{
		inputActions.AddRange(actionIds);
	}

	void DoAction(ActionData action, int startFrame = 0)
	{
		CurrentAction = action;
		CurrentFrame = 0;
		animator.CrossFade(action.id.ToString(), (float)startFrame * 2 / 100);
	}
	void DoAction(ActionId actionId, int startFrame = 0)
	{
		ActionData action = ActionTable.GetAction(actionId);
		DoAction(action, startFrame);
	}

}
