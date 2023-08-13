using System;
using System.Collections.Generic;
using System.Text;
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
    public string text;
    public int characterId; //人物
    public int expressionId; //表情
    public bool finishSign;

    public override string ToString()
    {
        return string.Format("character{0}: {1}", characterId, text);
    }
}

public static class ScriptTable
{
    static public List<ScriptData<ScriptDialog>> dialogTable = new()
    {
        new()
        {
            id = 0,
            data = new()
            {
                new() { id = 0, text = "测试1", characterId = 1, expressionId = 0 },
            }
        },
        new()
        {
            id = 1,
            data = new()
            {
                new() { id = 0, text = "测试2", characterId = 1, expressionId = 0 },
            }
        },
        new()
        {
            id = 2,
            data = new()
            {
                new() { id = 0, text = "我要快些回家", characterId = 1, expressionId = 0, finishSign = false },
                new() { id = 1, text = "似乎有什么事情发生了", characterId = 1, expressionId = 0, finishSign = true },
            }
        },
        new()
        {
            id = 3,
            data = new()
            {
                new() { id = 0, text = "看上去形势不妙", characterId = 1, expressionId = 0, finishSign = false },
                new() { id = 1, text = "希望我能赶得上", characterId = 1, expressionId = 0, finishSign = true },
            }
        },
    };

    static public ScriptData<T> GetScript<T>(int scriptId, ScriptType type)
    {
        object data = null;
        switch (type)
        {
            case ScriptType.Dialog: data = dialogTable[scriptId]; break;
        }
        return (ScriptData<T>)Convert.ChangeType(data, typeof(ScriptData<T>));
    }
}
