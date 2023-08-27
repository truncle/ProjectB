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

    private GameObject window;
    private string windowId;
    private string windowName;

    private int currStep = 0;
    private int statusSign = 0;

    private string GetDialogWindowName(ScriptDialog dialog)
    {
        return dialog.followTextIds == null ? "text" : "selection";
    }

    public int ReadScript(int scriptId, int type, int step)
    {
        InputManager.DisableInput();
        switch ((ScriptType)type)
        {
            case ScriptType.Dialog: ReadDialog(scriptId, step); break;
        }
        Debug.Log(string.Format("Trigger script id:{0}, type:{1}, step:{2}", scriptId, ((ScriptType)type).ToString(), currStep));
        return currStep;
    }

    public int UpdateScript(int scriptId, int type, int prevStep, out EventStatus status)
    {
        status = CheckStatus();
        if (status != EventStatus.Running)
        {
            return currStep;
        }
        switch ((ScriptType)type)
        {
            case ScriptType.Dialog: ReadDialog(scriptId, currStep + 1); break;
        }
        Debug.Log(string.Format("Update script id:{0}, type:{1}, step:{2}", scriptId, ((ScriptType)type).ToString(), currStep));
        status = EventStatus.Running;
        return currStep;
    }

    public void ChangeText(string text)
    {
        TextMeshProUGUI tmp = window.GetComponentInChildren<TextMeshProUGUI>();
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
        ScriptDialog dialog = scriptData.data.Where((data) => data.step == step).First();
        string windowName = GetDialogWindowName(dialog);
        if (!window || windowName != this.windowName)
        {
            if (window) WindowManager.Instance.CloseWindow(windowId);
            if (dialog.followTextIds == null)
            {
                //普通对话文本框
                windowId = WindowManager.Instance.OpenWindow(windowName);
                window = WindowManager.Instance.GetWindow(windowId);
                ChangeText(dialog.ToString());
            }
            else
            {
                //选项对话文本框
                windowId = WindowManager.Instance.OpenWindow(windowName);
                window = WindowManager.Instance.GetWindow(windowId);
                //特殊的文本框做特殊处理
                SelectionWindow selectionWindow = window.GetComponent<SelectionWindow>();
                TextMeshProUGUI[] tmps = window.GetComponentsInChildren<TextMeshProUGUI>();
                for (int i = 0; i < tmps.Length; i++)
                {
                    tmps[i].SetText(dialog.optionText[i]);
                }
                selectionWindow.scriptId = scriptId;
                //TextMeshProUGUI tmp1 = window.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
                //TextMeshProUGUI tmp2 = window.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
                //tmp1.SetText(dialoagData.optionDialog[0]);
                //tmp2.SetText(dialoagData.optionDialog[1]);
            }
            this.windowName = windowName;
        }
        else
        {
            ChangeText(dialog.ToString());
        }
        statusSign = dialog.status;
        currStep = dialog.step;
    }

    public void SelectDialog(int scriptId, int option)
    {
        ScriptData<ScriptDialog> scriptData = ScriptTable.GetScript<ScriptDialog>(scriptId, ScriptType.Dialog);
        ScriptDialog dialoagData = scriptData.data[currStep];
        int nextDialogId = dialoagData.followTextIds[option];
        ScriptDialog nextDialoagData = scriptData.data.Where((data) => data.id == nextDialogId).First();
        ReadDialog(scriptId, nextDialoagData.step);
    }
}

public enum ScriptType
{
    Dialog, Statement,
}
