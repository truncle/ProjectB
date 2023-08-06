using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class WindowManager
{
    static private WindowManager instance = new WindowManager();
    static public WindowManager Instance
    {
        get
        {
            return instance;
        }
    }

    private Dictionary<string, GameObject> windowPrefabs = new();

    private Dictionary<string, GameObject> currWindows = new();

    private void LoadWindowPrefab(string name)
    {
        windowPrefabs.Add(name, Util.LoadPrefab(GameConfig.windowNameToPath[name]));
    }

    private void UnloadWindowPrefab(string name)
    {
        windowPrefabs.Remove(name);
    }

    public string OpenWindow(string name)
    {
        if (!windowPrefabs.ContainsKey(name))
        {
            LoadWindowPrefab(name);
        }
        GameObject newWindow = Object.Instantiate(windowPrefabs[name]);
        var id = System.Guid.NewGuid().ToString();
        currWindows.Add(id, newWindow);
        return id;
    }

    public void CloseWindow(string id, bool unload = false)
    {
        Object.Destroy(currWindows[id]);
        currWindows.Remove(id);
        if (unload)
        {
            UnloadWindowPrefab(id);
        }
    }

    public void ShowWindow(string id)
    {
        currWindows[id].SetActive(false);
    }

    public void DisappearWindow(string id)
    {
        currWindows[id].SetActive(true);
    }

    public GameObject GetWindow(string id)
    {
        return currWindows[id];
    }
}
