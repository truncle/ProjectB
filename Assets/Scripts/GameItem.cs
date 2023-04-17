using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class GameItem : MonoBehaviour
{
	public int itemId = 0;

	private bool canPick = false;

	private GameObject playerObject;

	private void Awake()
	{
		Collider2D collider = GetComponent<Collider2D>();
		//todo 并非合适方式
		if (collider == null)
		{
			collider = gameObject.AddComponent<BoxCollider2D>();
			collider.isTrigger = true;
		}
	}

	void Update()
	{
		if (canPick)
		{
			CheckPick();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canPick = true;
			playerObject = collision.gameObject;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canPick = false;
		}
	}

	private void CheckPick()
	{
		if (itemId > 0)
		{
			ItemData itemData = ItemTable.GetItem(itemId);
			//todo 拾取道具可能会触发事件, 可能会立即生效, 需要判断是否需要交互拾取。
			if (canPick && (itemData.autoPick || InputManager.GetKeyDown(InputType.Interact)))
			{
				PickItem(itemData);
			}
		}
	}

	public void PickItem(int itemId)
	{
		ItemData itemData = ItemTable.GetItem(itemId);
		PickItem(itemData);
	}

	public void PickItem(ItemData itemData)
	{
		Debug.Log("pick up item");
		PlayerInventory.AddItem(itemId);
		if (itemData.eventId > 0)
		{
			//todo 如果有拾取事件, 执行事件
			Debug.Log("do pick up item event");
		}
		Destroy(gameObject);
	}
}
