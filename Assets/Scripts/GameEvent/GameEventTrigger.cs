using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameEventTrigger : MonoBehaviour
{
	public int eventId = 0;

	private bool canTrigger = false;

	private EventStatus eventStatus = EventStatus.NotTriggered;

	// Start is called before the first frame update
	void Start()
	{
		Collider2D collider = GetComponent<Collider2D>();
		//todo 并非合适方式
		if (collider == null)
		{
			collider = gameObject.AddComponent<BoxCollider2D>();
			collider.isTrigger = true;
		}
	}

	// Update is called once per frame
	void Update()
	{
		CheckTriggerEvent();
		CheckUpdateEvent();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (eventStatus != EventStatus.NotTriggered)
			return;
		//其他对象也可以触发事件？
		if (collision.CompareTag("Player"))
		{
			canTrigger = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canTrigger = false;
		}
	}

	private void CheckTriggerEvent()
	{
		GameEventData eventData = GameEventTable.GetEvent(eventId);
		//todo 拾取道具可能会触发事件, 可能会立即生效, 需要判断是否需要交互拾取。
		if (canTrigger && (!GameEventTable.NeedInteract(eventData.type) || InputManager.GetKeyDown(InputType.Interact)))
		{
			TriggerEvent(eventData);
		}
	}

	//todo 推进事件的方式，应该也会很复杂
	private void CheckUpdateEvent()
	{
		if (eventStatus != EventStatus.Running)
			return;
		if (InputManager.GetKeyDown(InputType.OK, true))
		{
			eventStatus = EventStatus.Triggered;
			InputManager.EnableInput();
		}
		//todo是否需要删除事件？
	}

	private void TriggerEvent(int eventId)
	{
		GameEventData eventData = GameEventTable.GetEvent(eventId);
		TriggerEvent(eventData);
	}

	//todo 执行事件的具体方式。应该需要非常复杂
	private void TriggerEvent(GameEventData eventData)
	{
		Debug.Log(string.Format("Trigger event, event text:{0}", eventData.eventText));
		if (eventData.blockInput)
		{
			eventStatus = EventStatus.Running;
			InputManager.DisableInput();
		}
		else
		{
			eventStatus = EventStatus.Triggered;
		}
		canTrigger = false;
	}
}
