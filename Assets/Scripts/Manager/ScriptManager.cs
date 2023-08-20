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

    private int currStep = 0;
    private int statusSign = 0;

    public int ReadScript(int scriptId, int type, int step)
    {
        InputManager.DisableInput();
        if (!textWindow)
        {
            windowId = WindowManager.Instance.OpenWindow("test");
            textWindow = WindowManager.Instance.GetWindow(windowId);
        }
        switch ((ScriptType)type)
        {
            case ScriptType.Dialog: ReadDialog(scriptId, step); break;
        }
        Debug.Log(string.Format("Trigger script id:{0}, type:{1}, step:{2}", scriptId, ((ScriptType)type).ToString(), currStep));
        return currStep;
    }

    public int UpdateScript(int scriptId, int type, int step, out EventStatus status)
    {
        status = CheckStatus();
        if (status != EventStatus.Running)
        {
            return currStep;
        }
        switch ((ScriptType)type)
        {
            case ScriptType.Dialog: ReadDialog(scriptId, step); break;
        }
        Debug.Log(string.Format("Update script id:{0}, type:{1}, step:{2}", scriptId, ((ScriptType)type).ToString(), step));
        status = EventStatus.Running;
        return currStep;
    }

    public void ChangeText(string text)
    {
        TextMeshProUGUI tmp = textWindow.GetComponentInChildren<TextMeshProUGUI>();
        tmp.SetText(text);
    }

    public EventStatus CheckStatus()
    {
        EventStatus status = EventStatus.Running;
        if (statusSign != 0)
        {
            WindowManager.Instance.CloseWindow(windowId);
            InputManager.EnableInput();
            switch (statusSign)
            {
                case 1: status = EventStatus.Triggered; break;
                case 2:
                    status = EventStatus.NotTriggered;
                    currStep = 0;
                    break;
            }
            statusSign = 0;
            return status;
        }
        return status;
    }

    public void ReadDialog(int scriptId, int step)
    {
        ScriptData<ScriptDialog> scriptData = ScriptTable.GetScript<ScriptDialog>(scriptId, ScriptType.Dialog);
        ScriptDialog dialoagData = scriptData.data[step];
        ChangeText(dialoagData.ToString());
        statusSign = dialoagData.status;
        currStep = step + 1;//todo 改成读表
    }
}

public enum ScriptType
{
    Dialog, Statement,
}
