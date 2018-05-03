/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private Dictionary<string, Action<System.Object>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<System.Object>>();
        }
    }

    public static void StartListening(string eventName, Action<System.Object> listener)
    {
        Action<System.Object> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<System.Object> listener)
    {
        if (eventManager == null) return;
        Action<System.Object> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, System.Object eventParam)
    {
        Action<System.Object> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventParam);
            // OR USE  instance.eventDictionary[eventName](eventParam);
        }
    }
}

//Re-usable structure/ Can be a class to. Add all parameters you need inside it
public struct EventParam
{
    public string param1;
    public int param2;
    public float param3;
    public bool param4;
}

*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[Serializable]
public class Events : UnityEvent<System.Object> { }

public class EventManager : MonoBehaviour
{

    public static EventManager Instance;

    private Dictionary<string, Events> _eventDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _eventDictionary = new Dictionary<string, Events>();
        }
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public static void StartListening(string eventName, UnityAction<System.Object> listener)
    {
        Events thisEvent = null;
        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new Events();
            thisEvent.AddListener(listener);
            Instance._eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<System.Object> listener)
    {
        if (Instance == null) return;
        Events thisEvent = null;
        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, System.Object arg = null)
    {
        Events thisEvent = null;
        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(arg);
        }
    }
}