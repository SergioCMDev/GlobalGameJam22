using System;
using System.Collections.Generic;
using Object = System.Object;

public class EventManager : IEventManager
{
    private readonly Dictionary<SubscriberData, Action<Object>> _subscribersActionsToDo;

    public EventManager()
    {
        _subscribersActionsToDo = new Dictionary<SubscriberData, Action<Object>>();
    }

    public void AddActionToSignal<T>(Action<T> actionToDo, Type publisher)
    {
        //TODO DO AN RANDOM IDENTIFIER
        Action<object> wrapperCallback = args => actionToDo((T) args);

        Add(typeof(T), publisher, wrapperCallback);
    }

    public void NotifySubscribers<T>(T signal, Type publisher)
    {
        Notify(signal, publisher);
    }

    public void RemoveActionFromSignal<T>(Action<T> changeLifeValue, Type publisher)
    {
        Action<object> wrapperCallback = args => changeLifeValue((T) args);

        RemoveAction(typeof(T), publisher, wrapperCallback);
    }

    private void Add(Type type, Type publisher, Action<Object> actionToDo)
    {
        SubscriberData data = new SubscriberData() {SignalType = type, Subscriber = publisher};

        if (_subscribersActionsToDo.ContainsKey(data))
        {
            return;
        }

        _subscribersActionsToDo.Add(data, actionToDo);
    }


    private void Notify(Object signal, Type publisher)
    {
        SubscriberData data = new SubscriberData() {SignalType = signal.GetType(), Subscriber = publisher};

        foreach (var VARIABLE in _subscribersActionsToDo.Keys)
        {
            if (VARIABLE.SignalType == signal.GetType())
            {
                _subscribersActionsToDo[VARIABLE].Invoke(signal);
            }
        }
        
    }

    private void RemoveAction(Type type, Type publisher, Action<object> wrapperCallback)
    {
        SubscriberData data = new SubscriberData() {SignalType = type, Subscriber = publisher};

        _subscribersActionsToDo.Remove(data);
    }
}