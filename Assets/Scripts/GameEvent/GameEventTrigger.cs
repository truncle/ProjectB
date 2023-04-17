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
		//todo ���Ǻ��ʷ�ʽ
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
		//��������Ҳ���Դ����¼���
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
		//todo ʰȡ���߿��ܻᴥ���¼�, ���ܻ�������Ч, ��Ҫ�ж��Ƿ���Ҫ����ʰȡ��
		if (canTrigger && (!GameEventTable.NeedInteract(eventData.type) || InputManager.GetKeyDown(InputType.Interact)))
		{
			TriggerEvent(eventData);
		}
	}

	//todo �ƽ��¼��ķ�ʽ��Ӧ��Ҳ��ܸ���
	private void CheckUpdateEvent()
	{
		if (eventStatus != EventStatus.Running)
			return;
		if (InputManager.GetKeyDown(InputType.OK, true))
		{
			eventStatus = EventStatus.Triggered;
			InputManager.EnableInput();
		}
		//todo�Ƿ���Ҫɾ���¼���
	}

	private void TriggerEvent(int eventId)
	{
		GameEventData eventData = GameEventTable.GetEvent(eventId);
		TriggerEvent(eventData);
	}

	//todo ִ���¼��ľ��巽ʽ��Ӧ����Ҫ�ǳ�����
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
