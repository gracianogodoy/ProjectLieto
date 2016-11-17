using UnityEngine;
using System;

public class TalkCallback : MonoBehaviour
{
    public Action OnTalkFinish;

    public void TalkFinish()
    {
        if (OnTalkFinish != null)
            OnTalkFinish();
    }
}
