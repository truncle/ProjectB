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
	public bool blockInput;

	public int scriptId; //代表事件指向一段剧本(对话等)
}

//todo 一些事件类型，如对话(npc交互，场景交互，剧情推进),拾取道具,场景切换
public enum EventType
{
	Normal, Script, PickItem
}

// 事件状态
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
		},
		new(){
			id = 1,
			type = EventType.Script,
			scriptId = 2,
			blockInput = true,
		},
		new(){
			id = 2,
			type = EventType.PickItem,
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

