using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIEvents
{
    public abstract class CustomUIEventBase<T> where T : EventBase
    {
        public CustomUIEventBase(Action callback, VisualElement element)
        {
            MethodInfo registerCallbackMethod = typeof(VisualElement).GetMethods()
                .Where(x => x.Name == "RegisterCallback")
                .FirstOrDefault(x => x.IsGenericMethod)
                .MakeGenericMethod(typeof(T));
            EventCallback<T> c = t =>
            {
                if (ExtractInfo(t)) callback.Invoke();
            };
            registerCallbackMethod.Invoke(element, new object[] { c, TrickleDown.TrickleDown });
        }
        abstract protected bool ExtractInfo(T eventType);
    }
    public class MouseRightClickEvent : CustomUIEventBase<MouseDownEvent>
    {
        public MouseRightClickEvent(Action callback, VisualElement element) : base(callback, element) { }

        protected override bool ExtractInfo(MouseDownEvent eventType)
        {
            return eventType.button == 1;
        }
    }
    public class MouseMiddleClickEvent : CustomUIEventBase<MouseDownEvent>
    {
        public MouseMiddleClickEvent(Action callback, VisualElement element) : base(callback, element) { }

        protected override bool ExtractInfo(MouseDownEvent eventType)
        {
            return eventType.button == 2;
        }
    }
    public class MouseDoubleClickEvent : CustomUIEventBase<MouseDownEvent>
    {
        public MouseDoubleClickEvent(Action callback, VisualElement element) : base(callback, element) { }

        protected override bool ExtractInfo(MouseDownEvent eventType)
        {
            return eventType.clickCount == 2;
        }
    }
    public class MouseDoubleLeftClickEvent : CustomUIEventBase<MouseDownEvent>
    {
        public MouseDoubleLeftClickEvent(Action callback, VisualElement element) : base(callback, element) { }

        protected override bool ExtractInfo(MouseDownEvent eventType)
        {
            return eventType.button == 0 && eventType.clickCount == 2;
        }
    }
    public class MouseDoubleRightClickEvent : CustomUIEventBase<MouseDownEvent>
    {
        public MouseDoubleRightClickEvent(Action callback, VisualElement element) : base(callback, element) { }

        protected override bool ExtractInfo(MouseDownEvent eventType)
        {
            return eventType.button == 1 && eventType.clickCount == 2;
        }
    }
    public class Key_Space_PressedEvent : CustomUIEventBase<KeyDownEvent>
    {
        public Key_Space_PressedEvent(Action callback, VisualElement element) : base(callback, element) { }

        protected override bool ExtractInfo(KeyDownEvent eventType)
        {
            return eventType.keyCode == KeyCode.Space;
        }
    }
    public class Key_Enter_PressedEvent : CustomUIEventBase<KeyDownEvent>
    {
        public Key_Enter_PressedEvent(Action callback, VisualElement element) : base(callback, element) { }

        protected override bool ExtractInfo(KeyDownEvent eventType)
        {
            return eventType.keyCode == KeyCode.Return;
        }
    }
}
