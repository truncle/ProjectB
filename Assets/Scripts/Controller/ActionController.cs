using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ActionController : MonoBehaviour
{
	protected Animator animator;
	//protected PhysicalController physicalController;
	protected Rigidbody2D rb;

	public string actionTableName;

	public int CurrentFrame { get; set; }

	public ActionData CurrentAction { get; set; }

	//触发类动作超时后回退的状态
	public string StateAction { get; set; }

	protected List<string> inputActions = new();

	public GameObject AttackedFX;

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();

		CurrentAction = ActionTable.DefaultAction(actionTableName);
	}

	// Update is called once per frame
	void Update()
	{
		//animator.SetFloat("SpeedX", Mathf.Abs(rb.velocity.x));
		//animator.SetFloat("SpeedY", rb.velocity.y);
	}

	void FixedUpdate()
	{
		CurrentFrame++;
		string nextAction = null;
		int startFrame = 0;
		//超时切换, 需要知道下一个状态或者需要回退的状态
		if (CurrentAction.type != ActionType.State && CurrentFrame > CurrentAction.totalFrames)
		{
			if (CurrentAction.nextAction != null)
				nextAction = CurrentAction.nextAction;
			else
				nextAction = StateAction;
		}

		//输入切换, 需要检查是否可以Cancel, 部分动作强制切换(受击, 死亡)
		if (inputActions.Count > 0)
			nextAction = inputActions.First();
		inputActions.Clear();

		if (nextAction != null)
			DoAction(nextAction, startFrame);
		//else
			//physicalController.DoAction(CurrentAction.name, CurrentFrame);
	}

	public void AddInputAction(List<string> inputActions)
	{
		this.inputActions.AddRange(inputActions);
	}

	void DoAction(ActionData? actionData, int startFrame = 0)
	{
		if (actionData.HasValue)
		{
			CurrentAction = actionData.Value;
			CurrentFrame = 0;
			animator.CrossFade(actionData.Value.name, (float)startFrame * 2 / 100);
            if (actionData.Value.name == "attacked")
            {
				AttackedFX.SetActive(true);
            }
            else
            {
				AttackedFX.SetActive(false);
            }
			//physicalController.DoAction(actionData.Value.name, startFrame);
		}
	}
	public void DoAction(string actionName, int startFrame = 0)
	{
		DoAction(ActionTable.GetAction(actionTableName, actionName), startFrame);
	}

}
