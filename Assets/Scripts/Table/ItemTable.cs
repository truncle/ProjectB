using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct ItemData
{
	public int id;
	public string name;
	public int eventId;
	public bool autoPick;
	//todo贴图？特效？拾取音效？使用道具所需数据？
}

static public class ItemTable
{
	static public List<ItemData> items = new(){
		new(){
			id = 0,
			name = "空道具",
		},
		new(){
			id = 1,
			name = "测试道具1",
			autoPick = true,
		},
		new(){
			id = 2,
			name = "测试道具2",
			eventId = 1,
			autoPick = false,
		},
	};

	static public ItemData GetItem(int itemId)
	{
		return items[itemId];
	}

}

