using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEventObject : MonoBehaviour
{
	//通过type和id来决定触发之后具体做什么
	[SerializeField]
	private EventType eventType;
	[SerializeField]
	private int id = 0;
	[SerializeField]
	private bool needInteract = true;

	//根据事件进程触发不同的逻辑
	public int process = 0;
	public EventStatus status;

	private bool canTrigger = false;

	public enum EventType
	{
		Script,
	}
	public enum EventStatus
	{
		NotTriggerd, Running, Triggered
	}

	void Awake()
	{
		Collider2D collider = GetComponent<Collider2D>();
		if (collider == null)
		{
			collider = gameObject.AddComponent<BoxCollider2D>();
			collider.isTrigger = true;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		CheckUpdateEvent();
		CheckTriggerEvent();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		//其他对象也可以触发事件？
		if (collision.CompareTag("Player") || collision.CompareTag("EventTrigger"))
		{
			canTrigger = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") || collision.CompareTag("EventTrigger"))
		{
			canTrigger = false;
		}
	}

	//只要在碰撞箱内且进度没满就持续触发事件
	private void CheckTriggerEvent()
	{
		if (canTrigger && status == EventStatus.NotTriggerd && (!needInteract || InputManager.GetKeyDown(InputType.Interact)))
		{
			TriggerEvent();
		}
	}

	private void CheckUpdateEvent()
	{
		if (status == EventStatus.Running && (InputManager.GetKeyDown(InputType.OK, true)))
		{
			TriggerEvent();
		}
	}

	//执行事件的具体方式
	private void TriggerEvent()
	{
		//不同的事件类型使用不同的触发方式，
		if (eventType == EventType.Script)
		{
			process = ScriptManager.Instance.ReadScript(id, process, out bool isFinish);
			if (isFinish)
				status = EventStatus.Triggered;
			else status = EventStatus.Running;
		}
	}
}
