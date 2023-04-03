using System.Collections.Generic;
using UnityEngine;


static class Util
{
	static public void PrintBox(Vector2 topLeft, Vector2 bottomRight)
	{
		PrintBox(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
	}

	static public void PrintBox(float x1, float y1, float x2, float y2)
	{
		Debug.DrawLine(new Vector2(x1, y1), new Vector2(x2, y1), Color.green);
		Debug.DrawLine(new Vector2(x1, y1), new Vector2(x1, y2), Color.green);
		Debug.DrawLine(new Vector2(x2, y2), new Vector2(x2, y1), Color.green);
		Debug.DrawLine(new Vector2(x2, y2), new Vector2(x1, y2), Color.green);
	}


	static public bool ContainsAll<T>(List<T> l1, List<T> l2)
	{
		if (l1.Count == 0 || l2 == null || l2.Count == 0 || l1.Count < l2.Count)
			return false;

		bool[] check = new bool[l1.Count];
		for (int i2 = 0; i2 < l2.Count; i2++)
		{
			bool found = false;
			for (int i1 = 0; i1 < l1.Count; i1++)
			{
				if (l2[i2].Equals(l1[i1]) && !check[i1])
				{
					check[i1] = true;
					found = true;
					break;
				}
			}
			if (!found)
				return false;
		}
		return true;
	}
}
