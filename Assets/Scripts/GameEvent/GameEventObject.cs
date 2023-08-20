using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEventObject : MonoBehaviour
{
    //通过type和id来决定触发之后具体做什么
    [SerializeField]
    private EventType eventType;
    [SerializeField]
    private List<int> idList;
    [SerializeField]
    private int idType;
    [SerializeField]
    private bool needInteract = true;

    //根据事件进程触发不同的逻辑
    public Dictionary<int, int> processMap = new();
    public Dictionary<int, EventStatus> statusMap = new();

    private bool canTrigger = false;

    public enum EventType
    {
        Script,
    }

    void Awake()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var id in idList)
        {
            if (CheckUpdateEvent(id, statusMap.GetValueOrDefault(id), processMap.GetValueOrDefault(id))) return;
        }
        foreach (var id in idList)
        {
            if (CheckTriggerEvent(id, statusMap.GetValueOrDefault(id), processMap.GetValueOrDefault(id))) return;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //其他对象也可以触发事件？
        if (collision.CompareTag("Player") || collision.CompareTag("EventTrigger"))
        {
            canTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("EventTrigger"))
        {
            canTrigger = false;
        }
    }

    //只要在碰撞箱内且进度没满就持续触发事件
    private bool CheckTriggerEvent(int id, EventStatus status, int process)
    {
        if (canTrigger && status == EventStatus.NotTriggered && (!needInteract || InputManager.GetKeyDown(InputType.Interact)))
        {
            TriggerEvent(id, process, false);
            return true;
        }
        return false;
    }

    private bool CheckUpdateEvent(int id, EventStatus status, int process)
    {
        if (status == EventStatus.Running && (InputManager.GetKeyDown(InputType.OK, true)))
        {
            TriggerEvent(id, process, true);
            return true;
        }
        return false;
    }

    //执行事件的具体方式
    private void TriggerEvent(int id, int process, bool isUpdate)
    {
        //不同的事件类型使用不同的触发方式，
        if (eventType == EventType.Script)
        {
            if (isUpdate)
            {
                processMap[id] = ScriptManager.Instance.UpdateScript(id, idType, process, out EventStatus status);
                statusMap[id] = status;
            }
            else
            {
                processMap[id] = ScriptManager.Instance.ReadScript(id, idType, process);
                statusMap[id] = EventStatus.Running;
            }
        }
    }
}
