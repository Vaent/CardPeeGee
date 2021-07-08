using UnityEditor;
using UnityEngine;

public class ManualTestHelper
{
    [MenuItem("DevTools/Quick Start #&P")]
    private static void QuickStart()
    {
        Debug.Log("QUICK START");
        GameState.QuickStart();
    }
}
