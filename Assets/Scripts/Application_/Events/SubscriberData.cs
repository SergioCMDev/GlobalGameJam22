using System;

namespace Application_.Events
{
    internal class SubscriberData : IEquatable<SubscriberData>
    {
        public Type SignalType { get; set; }
        public Object Subscriber { get; set; }

        public override bool Equals(object other)
        {
            if (other is SubscriberData)
            {
                SubscriberData otherId = (SubscriberData) other;
                return otherId.SignalType == this.SignalType;
            }

            return false;
        }

        public bool Equals(SubscriberData that)
        {
            return that.SignalType == this.SignalType;
        }

        public static bool operator ==(SubscriberData left, SubscriberData right)
        {
            return left.SignalType == right.SignalType && Equals(left.Subscriber, right.Subscriber);
        }

        public static bool operator !=(SubscriberData left, SubscriberData right)
        {
            return !(left == right);
        }
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + SignalType.GetHashCode();
                hash = hash * 29 + (Subscriber == null ? 0 : Subscriber.GetHashCode());
                return hash;
            }
        }
    }
}