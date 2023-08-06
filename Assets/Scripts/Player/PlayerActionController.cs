using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
	protected Animator animator;
	protected Rigidbody2D rb;

	public int CurrentFrame { get; set; }

	public PlayerActionData CurrentAction { get; set; }

	//触发类动作超时后回退的状态
	public ActionId StateActionId { get; set; }

	protected List<ActionId> inputActions = new();

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		CurrentAction = PlayerActionTable.GetAction(ActionId.Move).Value;
	}

	// Update is called once per frame
	void Update()
	{
		animator.SetFloat("SpeedX", Mathf.Abs(rb.velocity.x));
		animator.SetFloat("SpeedY", rb.velocity.y);
	}

	void FixedUpdate()
	{
		CurrentFrame++;
		ActionId? nextId = null;
		int startFrame = 0;
		//超时切换, 需要知道下一个状态或者需要回退的状态
		if (CurrentAction.type != ActionType.State && CurrentFrame > CurrentAction.totalFrames)
		{
			if (CurrentAction.nextId.HasValue)
				nextId = CurrentAction.nextId.Value;
			else
				nextId = StateActionId;
		}

		//输入切换, 需要检查是否可以Cancel, 部分动作强制切换(受击, 死亡)
		if (inputActions.Count > 0)
			nextId = CheckCancel(inputActions, out startFrame) ?? nextId;
		inputActions.Clear();

		if (nextId.HasValue)
			DoAction(nextId.Value, startFrame);
	}

	// todo 实现Cancel功能
	ActionId? CheckCancel(List<ActionId> actionIds, out int startFrame)
	{
		startFrame = 0;
		if (CurrentAction.cancelInfos == null || CurrentAction.cancelInfos.Count == 0)
			return actionIds.First();
		foreach (ActionId actionId in actionIds)
		{
			foreach (var cancelInfo in CurrentAction.cancelInfos)
			{
				if (cancelInfo.CanCancel(actionId, CurrentFrame))
				{
					startFrame = cancelInfo.toFrame;
					return cancelInfo.actionId;
				}
			}
		}
		return actionIds.First();
	}

	public void AddInputAction(List<ActionId> actionIds)
	{
		inputActions.AddRange(actionIds);
	}

	void DoAction(PlayerActionData? actionData, int startFrame = 0)
	{
		if (actionData.HasValue)
		{
			CurrentAction = actionData.Value;
			CurrentFrame = 0;
			animator.CrossFade(actionData.Value.id.ToString(), (float)startFrame * 2 / 100);
		}
	}
	void DoAction(ActionId actionId, int startFrame = 0)
	{
		DoAction(PlayerActionTable.GetAction(actionId), startFrame);
	}

}
