using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public struct ScriptData<T>
{
    public int id;
    public List<T> data;

    public string PrintScript()
    {
        if (data != null)
        {
            StringBuilder sb = new();
            foreach (T dataLine in data)
            {
                sb.AppendLine(dataLine.ToString());
            }
            Debug.Log(sb.ToString());
            return sb.ToString();
        }
        return null;
    }
}

public struct ScriptDialog
{
    public int id;
    public int boxId;
    public int characterId; //人物
    public string picturePath;
    public string soundPath;
    public int status;
    public int step;
    public string text;//通常对话文本
    public Dictionary<int, string> optionDialog;

    public string characterName; //对话框显示的角色名

    public override string ToString()
    {
        return string.Format("{0}: {1}", characterName, text);
        //return Util.PrintProperties(this);
    }
}

public static class ScriptTable
{
    static public List<ScriptData<ScriptDialog>> dialogTable = new();

    static public ScriptData<T> GetScript<T>(int scriptId, ScriptType type)
    {
        object data = null;
        switch (type)
        {
            case ScriptType.Dialog: data = dialogTable.Where((e) => e.id == scriptId).First(); break;
        }
        return (ScriptData<T>)Convert.ChangeType(data, typeof(ScriptData<T>));
    }

    static public void LoadTable()
    {
        LoadDialog();
    }

    static private void LoadDialog()
    {
        //todo 先写在这里, 之后挪到GameConfig里, 可能有多张表需要同时读取
        //todo 还要再详细理一下, 有不够明确的的地方
        var talkPath = "dialogue_1_talk.csv";
        //var scenePath = "dialogue_1_scene.csv";
        var characterPath = "character.csv";
        var textPath = "text.csv";
        var textboxStylePath = "textbox_style.csv";
        var resourcePath = "dialogue_res.csv";

        var talkTable = Util.ReadCsv(talkPath);
        //var sceneTable = Util.ReadCsv(scenePath);
        var characterTable = Util.ReadCsv(characterPath);
        var textTable = Util.ReadCsv(textPath);
        var textboxStyleTable = Util.ReadCsv(textboxStylePath);
        var resourceTable = Util.ReadCsv(resourcePath);
        Dictionary<int, ScriptData<ScriptDialog>> resultTmp = new();
        foreach (var pair in talkTable)
        {
            var dialogId = Convert.ToInt32(pair.Key);
            var dialogDataRaw = pair.Value;
            int scriptId = Convert.ToInt32(dialogDataRaw["group"]);
            var scriptData = resultTmp.ContainsKey(scriptId) ? resultTmp[scriptId] : new ScriptData<ScriptDialog>()
            {
                id = scriptId,
                data = new(),
            };

            var pictureId = dialogDataRaw["picId"];
            var soundId = dialogDataRaw["soundId"];
            var textId = dialogDataRaw["textId"];
            Dictionary<int, string> optionDialog = null;
            if (dialogDataRaw["followTextId"] != string.Empty)
            {
                optionDialog = new();
                var textIds = dialogDataRaw["textId"].Split("|");
                var followTextIds = dialogDataRaw["followTextId"].Split("|");
                for (var i = 0; i < followTextIds.Length; i++)
                {
                    optionDialog.Add(Convert.ToInt32(followTextIds[i]), textTable[textIds[i]]["text"]);
                }
            }
            ScriptDialog dialogData = new()
            {
                id = dialogId,
                boxId = Convert.ToInt32(dialogDataRaw["boxId"]),
                characterId = Convert.ToInt32(dialogDataRaw["characterId"]),
                picturePath = resourceTable.ContainsKey(pictureId) ? resourceTable[pictureId]["path"] : string.Empty,
                soundPath = resourceTable.ContainsKey(soundId) ? resourceTable[soundId]["path"] : string.Empty,
                status = Convert.ToInt32(dialogDataRaw["status"]),
                step = Convert.ToInt32(dialogDataRaw["step"]),
                text = textTable.ContainsKey(textId) ? textTable[textId]["text"] : string.Empty,
                optionDialog = optionDialog,

                characterName = textTable.ContainsKey(textId) ? textTable[textId]["name"] : string.Empty,
            };
            scriptData.data.Add(dialogData);
            resultTmp[scriptId] = scriptData;
        }

        foreach (var pair in resultTmp)
        {
            dialogTable.Add(pair.Value);
        }
    }
}
