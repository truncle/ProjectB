using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ScriptManager
{
	static private ScriptManager instance = new ScriptManager();
	static public ScriptManager Instance
	{
		get
		{
			return instance;
		}
	}

	private GameObject textWindow;
	private string windowId;

	public int ReadScript(int scriptId, int process, out bool isFinish)
	{
		InputManager.DisableInput();
		if (!textWindow)
		{
			windowId = WindowManager.Instance.OpenWindow("test");
			textWindow = WindowManager.Instance.GetWindow(windowId);
		}
		string scriptText = ScriptTable.GetScript(scriptId).PrintScript();
		ChangeText(scriptText);
		Debug.Log(string.Format("Trigger script id:{0}, process:{1}", scriptId, process));
		if (process >= 4)
		{
			isFinish = true;
			WindowManager.Instance.CloseWindow(windowId);
			InputManager.EnableInput();
		}
		else isFinish = false;
		process++;
		return process;
	}

	public void ChangeText(string text)
    {
		TextMeshProUGUI tmp = textWindow.GetComponentInChildren<TextMeshProUGUI>();
		tmp.SetText(text);
    }
}
