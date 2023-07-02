using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
	private Animator animator;
	private Rigidbody2D playerRb; 

	public int CurrentFrame { get; set; }

	public ActionData CurrentAction { get; set; }

	private List<ActionId> inputActions = new();

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		playerRb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		animator.SetFloat("SpeedX", Mathf.Abs(playerRb.velocity.x));
		animator.SetFloat("SpeedY", playerRb.velocity.y);
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
			Debug.Log(nextAction.id);
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
		Debug.Log(actionId);
		ActionData action = ActionTable.GetAction(actionId);
		DoAction(action, startFrame);
	}

}
