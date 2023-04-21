using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class PlayerInventory
{
	public static Dictionary<int, int> itemList = new();

	public static void AddItem(int itemId, int num = 1)
	{
		int itemNum = itemList.GetValueOrDefault(itemId, 0);
		itemList.Add(itemId, itemNum + num);
	}

	public static void RemoveItem(int itemId, int num = 1)
	{
		int itemNum = itemList.GetValueOrDefault(itemId, 0);
		if (itemNum <= num)
		{
			itemNum = 0;
			Debug.LogError("wrong item num!");
		}
		else
		{
			itemNum -= num;
		}
		itemList.Add(itemId, itemNum);
	}

	public static void UseItem(int itemId, int num = 1)
	{
		//todo 道具的使用功能
	}
}
