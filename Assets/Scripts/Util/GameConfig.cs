using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class GameConfig
{
    static public Dictionary<string, string> windowNameToPath = new()
    {
        { "text", "UI TextWindow" },
        { "selection", "UI SelectionWindow" }
    };
}
