using LetterDrop.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LetterDrop
{
    public interface IEventRegistrant
    {
        public void Deregister();
    }

    public class EventRegistrant : IEventRegistrant
    {
        public EventRegistrant(Action ev)
        {
            _event.SetTarget(ev);
        }
        public EventRegistrant(Action ev, Action handler) : this(ev)
        {
            Register(handler);
        }

        public void Register(Action handler)
        {
            // Try to get the handler
            Action currHandler = null;
            bool hasHandler = _handler.TryGetTarget(out currHandler);

            // If there is already a handler, don't register another one
            if (DebugUtils.AssertFalse(currHandler != null))
            {
                return;
            }

            // Store the handler
            _handler.SetTarget(handler);

            // Try to get the event
            Action ev = null;
            bool hasEvent = _event.TryGetTarget(out ev);

            // Register the handler
            if (hasEvent && ev != null)
            {
                ev += handler;
            }
        }

        public void Deregister()
        {
            // Get the event and handler
            Action handler = null;
            bool hasHandler = _handler.TryGetTarget(out handler);
            Action ev = null;
            bool hasEvent = _event.TryGetTarget(out ev);

            // Deregister if possible
            if (hasHandler && handler != null &&
                hasEvent && ev != null)
            {
                ev += handler;
            }
        }

        private WeakReference<Action> _event;
        private WeakReference<Action> _handler = null;
    }
}
