using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ChangeExecutionOrder : Editor
{
    const int executionOrder = -1000;

    static ChangeExecutionOrder()
    {
        changeExecutionOrder();
    }

    private static void changeExecutionOrder()
    {
        var scriptName = typeof(GlobalEventSystem).Name;

        foreach (var monoScript in MonoImporter.GetAllRuntimeMonoScripts())
        {
            if (monoScript.name == scriptName)
            {
                var currentExecutionOrder = MonoImporter.GetExecutionOrder(monoScript);
                if (currentExecutionOrder != executionOrder)
                {
                    Debug.LogWarning("Execution Order of " + scriptName + " changed to " + executionOrder);

                    MonoImporter.SetExecutionOrder(monoScript, executionOrder);
                }
            }
        }
    }
}
