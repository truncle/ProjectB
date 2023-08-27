using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionWindow : MonoBehaviour
{
    public int scriptId;
    public void Select(int option)
    {
        ScriptManager.Instance.SelectDialog(scriptId, option);
    }
}
