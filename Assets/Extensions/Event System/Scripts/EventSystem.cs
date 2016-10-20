using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.Assertions;

public interface IEvent
{
}

public class EventSystem : MonoBehaviour
{
    [SerializeField]
    private bool _limitQueueProcesing = false;
    [SerializeField]
    private float _queueProcessTime = 0.0f;
    private Queue _m_eventQueue = new Queue();

    public delegate void EventDelegate<T>(T e) where T : IEvent;
    private delegate void EventDelegate(IEvent e);

    private Dictionary<System.Type, EventDelegate> _delegates = new Dictionary<System.Type, EventDelegate>();
    private Dictionary<System.Delegate, EventDelegate> _delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

    #region MonoBehaviour Callbacks
    //Every update cycle the queue is processed, if the queue processing is limited,
    //a maximum processing time per update can be set after which the events will have
    //to be processed next update loop.
    public void Update()
    {
        float timer = 0.0f;
        while (_m_eventQueue.Count > 0)
        {
            if (_limitQueueProcesing)
            {
                if (timer > _queueProcessTime)
                    return;
            }

            IEvent evt = _m_eventQueue.Dequeue() as IEvent;
            TriggerEvent(evt);

            if (_limitQueueProcesing)
                timer += Time.deltaTime;
        }
    }

    public void OnApplicationQuit()
    {
        RemoveAll();
        _m_eventQueue.Clear();
    }
    #endregion

    private EventDelegate addDelegate<T>(EventDelegate<T> del) where T : IEvent
    {
        // Early-out if we've already registered this delegate
        if (_delegateLookup.ContainsKey(del))
            return null;

        // Create a new non-generic delegate which calls our generic one.
        // This is the delegate we actually invoke.
        EventDelegate internalDelegate = (e) => del((T)e);
        _delegateLookup[del] = internalDelegate;

        EventDelegate tempDel;
        if (_delegates.TryGetValue(typeof(T), out tempDel))
        {
            _delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            _delegates[typeof(T)] = internalDelegate;
        }

        return internalDelegate;
    }

    public void AddListener<T>(EventDelegate<T> del) where T : IEvent
    {
        addDelegate<T>(del);
    }

    public void RemoveListener<T>(EventDelegate<T> del) where T : IEvent
    {
        EventDelegate internalDelegate;
        if (_delegateLookup.TryGetValue(del, out internalDelegate))
        {
            EventDelegate tempDel;
            if (_delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;
                if (tempDel == null)
                {
                    _delegates.Remove(typeof(T));
                }
                else
                {
                    _delegates[typeof(T)] = tempDel;
                }
            }

            _delegateLookup.Remove(del);
        }
    }

    public void RemoveAll()
    {
        _delegates.Clear();
        _delegateLookup.Clear();
    }

    public bool HasListener<T>(EventDelegate<T> del) where T : IEvent
    {
        return _delegateLookup.ContainsKey(del);
    }

    public void TriggerEvent(IEvent e)
    {
        Assert.IsNotNull(e, "Event " + e.GetType().Name + " is null!");

        EventDelegate del;
        if (_delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
        }
        else
        {
            Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
        }
    }

    //Inserts the event into the current queue.
    public bool QueueEvent(IEvent e)
    {
        Assert.IsNotNull(e, "Event " + e.GetType().Name + " is null!");

        if (!_delegates.ContainsKey(e.GetType()))
        {
            Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + e.GetType());
            return false;
        }

        _m_eventQueue.Enqueue(e);
        return true;
    }
}
