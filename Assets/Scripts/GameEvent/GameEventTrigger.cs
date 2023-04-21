using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class GameEventTrigger : MonoBehaviour
{
	public int eventId = 0;

	private bool canTrigger = false;

	private EventStatus eventStatus = EventStatus.NotTriggered;

	private GameObject textWindow;

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
			if (textWindow)
			{
				Destroy(textWindow);
			}
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
		Debug.Log(string.Format("Trigger event id:{0}", eventData.id));
		if (eventData.type == EventType.Script)
		{
			string scriptText = ScriptTable.GetScript(eventData.scriptId).PrintScript();
			GameObject textWindowPrefab = Util.LoadPrefab("UI TextWindow");
			textWindow = Instantiate(textWindowPrefab);
			TextMeshProUGUI tmp = textWindow.GetComponentInChildren<TextMeshProUGUI>();
			tmp.SetText(scriptText);
		}

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
