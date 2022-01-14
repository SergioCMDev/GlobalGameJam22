using System;
using Utils;

namespace Application_.Signals
{
    public abstract class SignalBase
    {
        public void Execute(Type publisher)
        {
            ServiceLocator.Instance.GetService<IEventManager>().NotifySubscribers(this, publisher);
        }

    }
}