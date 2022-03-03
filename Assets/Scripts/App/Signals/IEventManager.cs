using System;

namespace App.Signals
{
    public interface IEventManager
    {
        void AddActionToSignal<T>(Action<T> actionToDo, Type publisher);
        void NotifySubscribers<T>(T signal, Type publisher);
        void RemoveActionFromSignal<T>(Action<T> changeLifeValue, Type publisher);
    }
}