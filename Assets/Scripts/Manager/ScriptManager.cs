using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ScriptManager
{
    static private ScriptManager instance = new();
    static public ScriptManager Instance
    {
        get
        {
            return instance;
        }
    }

    static public Dictionary<ScriptType, System.Type> scriptType2Type = new()
    {
        { ScriptType.Dialog, typeof(ScriptDialog) },
    };

    private GameObject textWindow;
    private string windowId;

    private bool finishSign = false;

    public int ReadScript(int scriptId, int type, int process)
    {
        InputManager.DisableInput();
        if (!textWindow)
        {
            windowId = WindowManager.Instance.OpenWindow("test");
            textWindow = WindowManager.Instance.GetWindow(windowId);
        }
        switch ((ScriptType)type)
        {
            case ScriptType.Dialog: ReadDialog(scriptId, process); break;
        }
        Debug.Log(string.Format("Trigger script id:{0}, type:{1}, process:{2}", scriptId, ((ScriptType)type).ToString(), process));
        return process;
    }

    public int UpdateScript(int scriptId, int type, int process, out bool isFinish)
    {
        if (isFinish = CheckFinish())
        {
            return process;
        }
        process++;
        switch ((ScriptType)type)
        {
            case ScriptType.Dialog: ReadDialog(scriptId, process); break;
        }
        Debug.Log(string.Format("Update script id:{0}, type:{1}, process:{2}", scriptId, ((ScriptType)type).ToString(), process));
        isFinish = false;
        return process;
    }

    public void ChangeText(string text)
    {
        TextMeshProUGUI tmp = textWindow.GetComponentInChildren<TextMeshProUGUI>();
        tmp.SetText(text);
    }

    public bool CheckFinish()
    {
        if (finishSign)
        {
            WindowManager.Instance.CloseWindow(windowId);
            InputManager.EnableInput();
            finishSign = false;
            return true;
        }
        return false;
    }

    public void ReadDialog(int scriptId, int process)
    {
        ScriptData<ScriptDialog> scriptData = ScriptTable.GetScript<ScriptDialog>(scriptId, ScriptType.Dialog);
        ScriptDialog dialoagData = scriptData.data[process];
        ChangeText(dialoagData.ToString());
        finishSign = dialoagData.finishSign;
    }
}

public enum ScriptType
{
    Dialog, Statement,
}
