using UnityEngine;

public class SetupResolution : MonoBehaviour
{
    void Start()
    {
        Debug.LogError("Defore: " + Screen.currentResolution);
        var screenRes = Screen.currentResolution;
        if (screenRes.width != 1920 || screenRes.height != 1080)
            Screen.SetResolution(1920, 1080, true);
        Debug.LogError("After: " + Screen.currentResolution);
    }
}
