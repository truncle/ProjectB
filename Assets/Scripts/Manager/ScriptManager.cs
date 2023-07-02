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

	public int ReadScript(int scriptId, int process, out bool isFinish)
	{
		InputManager.DisableInput();
		GameObject textWindowPrefab = Util.LoadPrefab("UI TextWindow");
		if (!textWindow)
			textWindow = Object.Instantiate(textWindowPrefab);
		TextMeshProUGUI tmp = textWindow.GetComponentInChildren<TextMeshProUGUI>();
		string scriptText = ScriptTable.GetScript(scriptId).PrintScript();
		tmp.SetText(scriptText);
		Debug.Log(string.Format("Trigger script id:{0}, process:{1}", scriptId, process));
		if (process >= 4)
		{
			isFinish = true;
			Object.Destroy(textWindow);
			InputManager.EnableInput();
		}
		else isFinish = false;
		process++;
		return process;
	}
}
