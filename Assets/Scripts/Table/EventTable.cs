using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//todo 触发方式(自动/交互/需要碰撞/条件达成)？推进方式？结束方式?事件关联?连续事件？条件事件？分支事件？重复触发？
public struct GameEventData
{
	public int id;
	public EventType type;
	public string eventText;
	public bool blockInput;
}

//todo 一些事件类型，如对话(npc交互，场景交互，剧情推进),拾取道具,场景切换
public enum EventType
{
	Normal, Talk, PickItem
}

public enum EventStatus
{
	NotTriggered, Running, Triggered,
}

public static class GameEventTable
{
	static public List<GameEventData> events = new(){
		new(){
			id = 0,
			type = EventType.Normal,
			eventText = "空事件",
		},
		new(){
			id = 1,
			type = EventType.Talk,
			eventText = "测试事件1:对话事件",//todo 生成对话框
			blockInput = true,
		},
		new(){
			id = 2,
			type = EventType.PickItem,
			eventText = "测试事件2:拾取道具事件",
		},
	};

	public static List<EventType> AutoTriggerTypes = new()
	{
		EventType.Normal,
	};

	static public GameEventData GetEvent(int eventId)
	{
		return events[eventId];
	}

	static public bool NeedInteract(EventType eventType)
	{
		return !AutoTriggerTypes.Contains(eventType);
	}
}

