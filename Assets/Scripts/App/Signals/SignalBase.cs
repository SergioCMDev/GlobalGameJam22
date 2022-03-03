using System;
using Utils;

namespace App.Signals
{
    public abstract class SignalBase
    {
        public void Execute(Type publisher)
        {
            ServiceLocator.Instance.GetService<IEventManager>().NotifySubscribers(this, publisher);
        }

    }
}