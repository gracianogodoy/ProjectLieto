using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(EventSystem))]
public class GlobalEventSystem : MonoBehaviour
{
    private static EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = GetComponent<EventSystem>();
    }

    public static void AddListener<T>(EventSystem.EventDelegate<T> del) where T : IEvent
    {
        _eventSystem.AddListener(del);
    }

    public static void RemoveListener<T>(EventSystem.EventDelegate<T> del) where T : IEvent
    {
        _eventSystem.RemoveListener(del);
    }

    public static void RemoveAll()
    {
        _eventSystem.RemoveAll();
    }

    public static bool HasListener<T>(EventSystem.EventDelegate<T> del) where T : IEvent
    {
        return _eventSystem.HasListener(del);
    }

    public static void TriggerEvent(IEvent e)
    {
        _eventSystem.TriggerEvent(e);
    }

    public static bool QueueEvent(IEvent e)
    {
        return _eventSystem.QueueEvent(e);
    }
}
