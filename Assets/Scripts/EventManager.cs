using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance = null;
    public static EventManager Instance()
    {
        if(_instance==null)
        {
            _instance = new EventManager();
        }
        return _instance;
    }
    public delegate void notification(Message message);
    private Dictionary<string, notification> listeners = new Dictionary<string, notification>();
    public void addListener(string s,notification noti)
    {
        if (!listeners.ContainsKey(s))
        {
            listeners.Add(s, noti);
        }
    }
    public void invokeListener(string s, Message message)
    {
        if(listeners.ContainsKey(s))
        {
            listeners[s](message);
        }
        else
        {
            Debug.Log("listener " + s + " does not exist");
            return;
        }
    }
}
public class Message
{
    public object var1, var2, var3;
    public GameObject sender;
    Message(GameObject sender,object v)
    {
        var1 = v;
        this.sender = sender;
    }
    Message(GameObject sender)
    {
        this.sender = sender;
    }
    Message(GameObject sender, object v1, object v2)
    {
        this.sender = sender;
        var1 = v1;
        var2 = v2;
    }
    Message(GameObject sender,object v1,object v2,object v3)
    {
        this.sender = sender;
        var1 = v1;
        var2 = v2;
        var3 = v3;
    }
}